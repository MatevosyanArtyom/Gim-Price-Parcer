using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Services.PriceLists;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Processor;
using Gim.PriceParser.Processor.RuntimeCompiler;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.PriceList;
using Gim.PriceParser.WebApi.Models.PriceListItem;
using Gim.PriceParser.WebApi.Models.SchedulerTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceListsController: ApiControllerBase
    {
        private readonly IRuntimeCompiler _compiler;
        private readonly IPriceListDao _dao;
        private readonly IMapper _mapper;
        private readonly IPriceListItemDao _priceListItemDao;
        private readonly IPriceListItemImageDao _priceListItemImageDao;
        private readonly IPriceListItemPropertyDao _priceListItemPropertyDao;
        private readonly IProcessingRuleDao _processingRuleDao;
        private readonly IProcessorClient _processorClient;
        private readonly IPriceListService _service;

        public PriceListsController(IRuntimeCompiler compiler, IPriceListDao dao, IMapper mapper, IPriceListItemDao priceListItemDao, IPriceListItemImageDao priceListItemImageDao, IPriceListItemPropertyDao priceListItemPropertyDao,
            IProcessingRuleDao processingRuleDao, IProcessorClient processorClient, IPriceListService service)
        {
            _compiler = compiler;
            _dao = dao;
            _mapper = mapper;
            _priceListItemDao = priceListItemDao;
            _priceListItemImageDao = priceListItemImageDao;
            _priceListItemPropertyDao = priceListItemPropertyDao;
            _processingRuleDao = processingRuleDao;
            _processorClient = processorClient;
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.PriceListsRead)]
        public async Task<GetAllResultDto<PriceListLookup>> GetMany([FromQuery] PriceListFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            if (filter.Status != PriceListStatus.Committed)
            {
                filter.ExceptStatus = PriceListStatus.Committed;
            }

            var docs = await _dao.GetManyAsync(filter, sort, false, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<PriceListLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.PriceListsRead)]
        public async Task<PriceListFull> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id, true);
            var docDto = _mapper.Map<PriceListFull>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.PriceListAdd)]
        public async Task<PriceListLookup> AddOne([FromBody] PriceListAdd entity)
        {
            var doc = _mapper.Map<PriceList>(entity);
            doc.AuthorId = CurrentUserId;

            // remove service info if exist, e.g. 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,'
            doc.PriceListFile.Data = doc.PriceListFile.Data.Split(',').Last();

            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<PriceListLookup>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> DeleteOne([FromQuery] string id)
        {
            if (!await CheckEditRights(id))
            {
                return Forbid();
            }

            await _dao.DeleteOneAsync(id);

            var itemsIds = await _priceListItemDao.DeleteManyAsync(id);

            var propsFilter = new PriceListItemPropertyFilter
            {
                PriceListItemsIds = itemsIds
            };
            await _priceListItemPropertyDao.DeleteManyAsync(propsFilter);

            var imagesFilter = new PriceListItemImageFilter
            {
                PriceListItemsIds = itemsIds
            };
            await _priceListItemImageDao.DeleteManyAsync(imagesFilter);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/commit")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> CommitOne([FromRoute] string id)
        {
            if (!await CheckEditRights(id))
            {
                return Forbid();
            }

            var doc = await _dao.GetOneAsync(id, true);

            if (doc.Status != PriceListStatus.Ready && doc.Status != PriceListStatus.Errors)
            {
                return BadRequest();
            }

            switch (doc.Status)
            {
                case PriceListStatus.Errors when doc.HasUnprocessedErrors:
                    return BadRequest();
                //case PriceListStatus.Errors when doc.HasPropertiesErrors && !HttpContext.User.IsInRole(KnownRoles.PriceListsCommitWithErrors):
                //    return Forbid();
                default:
                    await _processorClient.CommitPriceListAsync(id);
                    return Ok();
            }
        }

        [HttpPost]
        [Route("parse-one")]
        [Authorize(Roles = KnownRoles.PriceListsFull)]
        public async Task<List<PriceListItemLookup>> ParseOne([FromBody] PriceListAdd src)
        {
            var compileResult = _compiler.Compile(src.Code);

            // remove service info if exist, e.g. 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,'
            src.PriceListFile.Data = src.PriceListFile.Data.Split(',').Last();

            var items = Xlsx.Parse(compileResult.Assembly, src.PriceListFile.Data);


            var itemsMatched = await _processorClient.MatchItemsAsync(items, _dao.GenerateNewObjectId(), src.Supplier);

            var itemsDto = _mapper.Map<List<PriceListItemLookup>>(itemsMatched);

            //return itemsDto;
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("{id}/search-products")]
        [Authorize(Roles = KnownRoles.PriceListsFull)]
        public async Task SearchProducts([FromRoute] string id)
        {
            await _service.SearchProducts(id);
        }

        [HttpPost]
        [Route("test")]
        [Authorize(Roles = KnownRoles.PriceListsFull)]
        public async Task<IActionResult> Index([FromForm] IFormFile file1)
        {
            foreach (var file in Request.Form.Files)
            {
                var ext = Path.GetExtension(file.FileName)?.Replace(".", "");

                switch (ext)
                {
                    case "xls":
                    case "xlsx":
                        FromXls(file);
                        break;
                    case "csv":
                        FromCsv(file);
                        break;
                    default:
                        continue;
                }
            }

            await Task.CompletedTask;
            return Ok();
        }

        [HttpPost]
        [Route("emit")]
        public async Task<ActionResult<EmitResultDto>> CheckEmit([FromBody] string processingRuleId)
        {
            var processingRule = await _processingRuleDao.GetOneAsync(processingRuleId);

            var compileResult = _compiler.Compile(processingRule.Code, processingRule.RulesSource == RulesSource.Code ? Templates.Xlsx : null);

            var result = _mapper.Map<EmitResultDto>(compileResult.EmitResult);
            return result;
        }

        private static async void FromXls(IFormFile file)
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var ep = new ExcelPackage(stream);
            var sheets = ep.Workbook.Worksheets;
        }

        private static async void FromCsv(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var cells = line.Split(';');
            }
        }

        private async Task<bool> CheckEditRights(string id)
        {
            var doc = await _dao.GetOneAsync(id);

            // Можно редактировать только "свои" прайс-листы, либо есть полные права
            return doc.AuthorId == CurrentUserId || HttpContext.User.IsInRole(KnownRoles.PriceListsFull);
        }
    }
}
