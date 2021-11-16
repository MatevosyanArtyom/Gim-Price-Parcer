using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Mail;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao _dao;
        private readonly IPasswordHasher<GimUser> _hasher;
        private readonly MailSettings _mailSettings;
        private readonly IMailClient _mailClient;
        private readonly IMapper _mapper;

        public UsersController(IUserDao dao, IMapper mapper, IPasswordHasher<GimUser> hasher, IOptions<MailSettings> mailOptions, IMailClient mailClient)
        {
            _dao = dao;
            _mapper = mapper;
            _hasher = hasher;
            _mailSettings = mailOptions.Value;
            _mailClient = mailClient;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.UsersRead)]
        public async Task<GetAllResultDto<UserLookup>> GetMany([FromQuery] GimUserFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<UserLookup>>(docs);

            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.UsersRead)]
        public async Task<UserEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<UserEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.UsersFull)]
        public async Task<UserEdit> AddOne([FromBody] UserAdd entity)
        {
            var doc = _mapper.Map<GimUser>(entity);
            doc.Password = _hasher.HashPassword(doc, doc.Password);
            doc.ChangePasswordToken = _dao.GenerateNewObjectId();
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<UserEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.UsersFull)]
        public async Task<UserEdit> UpdateOne([FromBody] UserEdit entity)
        {
            var doc = _mapper.Map<GimUser>(entity);

            // Restore old password
            var oldUser = await _dao.GetOneAsync(entity.Id);
            doc.Password = oldUser.Password;

            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<UserEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("new-password-token/{id}")]
        [Authorize(Roles = KnownRoles.UsersFull)]
        public async Task<string> SetPasswordToken([FromRoute] string id)
        {
            var doc = await _dao.SetPasswordTokenAsync(id);

            var hostName = string.IsNullOrWhiteSpace(_mailSettings.HostName) ? Request.Host.ToString(): _mailSettings.HostName; 
            var link = $"http://{hostName}/passwordChange/{doc.ChangePasswordToken}";
            var html = $"<a href=\"{link}\">{link}</a>";

            await _mailClient.SendMessageAsync(doc.Email, "Ссылка для смены пароля", html);

            return doc.ChangePasswordToken;
        }

        [HttpPatch]
        [Route("to-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.UsersFull)]
        public async Task ToArchiveOne([FromRoute] string id)
        {
            await _dao.ToArchiveOneAsync(id);
        }

        [HttpPatch]
        [Route("from-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.UsersFull)]
        public async Task FromArchiveOne([FromRoute] string id)
        {
            await _dao.FromArchiveOneAsync(id);
        }
    }
}