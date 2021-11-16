using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Processor;
using Gim.PriceParser.Processor.RuntimeCompiler;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.ProcessingRule;
using Gim.PriceParser.WebApi.Models.SchedulerTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ProcessingRuleController: ControllerBase
    {
        private readonly IRuntimeCompiler _compiler;
        private readonly IProcessingRuleDao _dao;
        private readonly IMapper _mapper;

        public ProcessingRuleController(IRuntimeCompiler compiler, IProcessingRuleDao dao, IMapper mapper)
        {
            _compiler = compiler;
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.ProcessingRulesRead)]
        public async Task<GetAllResultDto<ProcessingRuleLookup>> GetMany([FromQuery] ProcessingRuleFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<ProcessingRuleLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.ProcessingRulesRead)]
        public async Task<ProcessingRuleFull> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<ProcessingRuleFull>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.ProcessingRulesFull)]
        public async Task<ProcessingRuleFull> AddOne([FromBody] ProcessingRuleAdd entity)
        {
            var doc = _mapper.Map<ProcessingRule>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<ProcessingRuleFull>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.ProcessingRulesFull)]
        public async Task<ProcessingRuleFull> UpdateOne([FromBody] ProcessingRuleFull entity)
        {
            var doc = _mapper.Map<ProcessingRule>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<ProcessingRuleFull>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("to-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.ProcessingRulesFull)]
        public async Task ToArchiveOne([FromRoute] string id)
        {
            await _dao.ToArchiveOneAsync(id);
        }

        [HttpPatch]
        [Route("from-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.ProcessingRulesFull)]
        public async Task FromArchiveOne([FromRoute] string id)
        {
            await _dao.FromArchiveOneAsync(id);
        }

        [HttpPost]
        [Route("emit")]
        public ActionResult<EmitResultDto> CheckEmit([FromBody] CheckEmitPayload payload)
        {
            var compileResult = _compiler.Compile(payload.Script, payload.RulesSource == RulesSource.Code ? Templates.Xlsx : null);

            var result = _mapper.Map<EmitResultDto>(compileResult.EmitResult);
            return result;
        }
    }
}
