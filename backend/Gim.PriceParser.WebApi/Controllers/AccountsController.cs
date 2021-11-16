using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ApiControllerBase
    {
        private const string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        private readonly IPasswordHasher<GimUser> _hasher;
        private readonly IMapper _mapper;
        private readonly IUserDao _userDao;

        public AccountsController(IPasswordHasher<GimUser> hasher, IUserDao userDao, IMapper mapper)
        {
            _hasher = hasher;
            _userDao = userDao;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var gimUser = await _userDao.GetOneByEmailAsync(loginModel.Email);
            if (gimUser == null)
            {
                return NotFound();
            }

            var signInResult = _hasher.VerifyHashedPassword(gimUser, gimUser.Password, loginModel.Password);

            if (signInResult != PasswordVerificationResult.Success)
            {
                return NotFound();
            }

            if (gimUser.Status == GimUserStatus.Blocked || gimUser.Status == GimUserStatus.SystemBlocked ||
                gimUser.IsArchived)
            {
                return Forbid();
            }

            await SignInAsync(gimUser);

            return Ok();
        }

        private async Task SignInAsync(GimUser gimUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, gimUser.Id)
            };

            claims.AddRange(KnownRoles.GetClaims(gimUser.Role));

            if (gimUser.HasSuppliersAccess)
            {
                claims.Add(new Claim(ClaimTypes.Role, KnownRoles.Manager));
            }

            if (gimUser.HasFullAccess)
            {
                claims.Add(new Claim(ClaimTypes.Role, KnownRoles.Moderator));
            }

            if (gimUser.HasUsersAccess)
            {
                claims.Add(new Claim(ClaimTypes.Role, KnownRoles.Admin));
            }

            var id = new ClaimsIdentity(claims, AuthenticationScheme);

            await HttpContext.SignInAsync(
                AuthenticationScheme,
                new ClaimsPrincipal(id)
            );
        }

        [HttpGet]
        [Route("token-validation")]
        [AllowAnonymous]
        public async Task<CheckTokenValidityModel> ValidateToken([FromQuery] string token)
        {
            var filter = new GimUserFilter
            {
                Token = token
            };

            var doc = await _userDao.GetOneAsync(filter);
            var result = new CheckTokenValidityModel
            {
                Token = token
            };

            if (doc == null)
            {
                return result;
            }

            result.IsTokenValid = true;
            result.Email = doc.Email;
            result.Status = doc.Status;

            return result;
        }

        [HttpPatch]
        [Route("new-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordAndLogin([FromBody] ChangePasswordModel model)
        {
            var filter = new GimUserFilter
            {
                Token = model.Token
            };

            var gimUser = await _userDao.GetOneAsync(filter);
            if (gimUser == null)
            {
                return BadRequest();
            }

            // Если это новый пользователь, нужно его сразу авторизовать
            var isNew = gimUser.Status == GimUserStatus.New;

            gimUser.Password = _hasher.HashPassword(gimUser, model.Password);
            gimUser.Status = GimUserStatus.Active;
            gimUser.ChangePasswordToken = null;

            gimUser = await _userDao.UpdateOneAsync(gimUser);

            if (isNew)
            {
                await SignInAsync(gimUser);
            }

            return Ok();
        }

        [HttpGet]
        [Route("userinfo")]
        [Authorize]
        public async Task<UserLookup> UserInfo()
        {
            var gimUser = await _userDao.GetOneAsync(CurrentUserId);
            return _mapper.Map<UserLookup>(gimUser);
        }
    }
}