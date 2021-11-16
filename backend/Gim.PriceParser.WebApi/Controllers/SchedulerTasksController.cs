using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.SchedulerTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerTasksController: ControllerBase
    {
        private readonly ISchedulerTaskDao _dao;
        private readonly IMapper _mapper;

        public SchedulerTasksController(ISchedulerTaskDao dao, IMapper mapper)
        {
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<SchedulerTaskLookup>> GetMany([FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<SchedulerTaskLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<SchedulerTaskFull> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<SchedulerTaskFull>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task<SchedulerTaskFull> AddOne([FromBody] SchedulerTaskAdd entity)
        {
            var doc = _mapper.Map<SchedulerTask>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<SchedulerTaskFull>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task<SchedulerTaskFull> UpdateOne([FromBody] SchedulerTaskEdit entity)
        {
            var doc = _mapper.Map<SchedulerTask>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<SchedulerTaskFull>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
        }
    }
}
