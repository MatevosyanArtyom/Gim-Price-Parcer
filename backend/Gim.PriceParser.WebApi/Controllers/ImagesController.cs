using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageDao _dao;
        private readonly IMapper _mapper;

        public ImagesController(IImageDao dao, IMapper mapper)
        {
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<ImageFullDto>> GetMany([FromQuery] GimImageFilter filter, int page,
            int pageSize)
        {
            var docs = await _dao.GetManyAsync(filter, page, pageSize, true);
            var docsDto = _mapper.Map<GetAllResultDto<ImageFullDto>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("main")]
        [Authorize]
        public async Task<ImageFullDto> GetMain([FromQuery] string productId)
        {
            var doc = await _dao.GetMainAsync(productId);
            var docDto = _mapper.Map<ImageFullDto>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<ImageLookupDto> AddOne([FromBody] ImageAddDto entity)
        {
            var gimImage = _mapper.Map<GimImage>(entity);
            gimImage = await _dao.AddOneAsync(gimImage);
            var imageDto = _mapper.Map<ImageLookupDto>(gimImage);
            return imageDto;
        }

        [HttpPatch]
        [Route("{id}/main")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task SetMain([FromRoute] string id)
        {
            await _dao.SetMainAsync(id);
        }

        [HttpPatch]
        [Route("{id}/publish")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task SetPublished([FromRoute] string id, bool isPublished)
        {
            await _dao.SetPublishedAsync(id, isPublished);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task DeleteOne([FromRoute] string id)
        {
            await _dao.DeleteOneAsync(id);
        }
    }
}