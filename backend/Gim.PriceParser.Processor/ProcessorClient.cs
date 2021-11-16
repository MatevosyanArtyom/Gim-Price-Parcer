using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Search;
using Gim.PriceParser.Bll.Services.Products;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Processor.RuntimeCompiler;

namespace Gim.PriceParser.Processor
{
    internal class ProcessorClient: IProcessorClient
    {
        private readonly ICategoryDao _categoryDao;
        private readonly ICategoryPropertyDao _categoryPropertyDao;
        private readonly IRuntimeCompiler _compiler;
        private readonly IImageDao _imageDao;
        private readonly IMapper _mapper;
        private readonly IPriceListDao _priceListDao;
        private readonly IPriceListItemDao _priceListItemDao;
        private readonly IPriceListItemImageDao _priceListItemImageDao;
        private readonly IPriceListItemPropertyDao _priceListItemPropertyDao;
        private readonly IProductDao _productDao;
        private readonly IProductService _productService;
        private readonly ICategoryPropertyValueDao _propertyValueDao;
        private readonly ISearchClient _searchClient;
        private readonly ISupplierProductDao _supplierProductDao;

        public ProcessorClient(ICategoryDao categoryDao, IRuntimeCompiler compiler, IImageDao imageDao, IMapper mapper,
            IPriceListDao priceListDao, IPriceListItemDao priceListItemDao,
            IPriceListItemPropertyDao priceListItemPropertyDao, IPriceListItemImageDao priceListItemImageDao,
            IProductDao productDao, IProductService productService, ICategoryPropertyDao categoryPropertyDao,
            ICategoryPropertyValueDao propertyValueDao, ISearchClient searchClient,
            ISupplierProductDao supplierProductDao)
        {
            _categoryDao = categoryDao;
            _compiler = compiler;
            _imageDao = imageDao;
            _mapper = mapper;
            _priceListDao = priceListDao;
            _priceListItemDao = priceListItemDao;
            _priceListItemImageDao = priceListItemImageDao;
            _priceListItemPropertyDao = priceListItemPropertyDao;
            _productDao = productDao;
            _productService = productService;
            _categoryPropertyDao = categoryPropertyDao;
            _propertyValueDao = propertyValueDao;
            _searchClient = searchClient;
            _supplierProductDao = supplierProductDao;
        }

        public async Task ParsePriceListsAsync()
        {
            var filter = new PriceListFilter
            {
                Status = PriceListStatus.InQueue
            };

            var pricelists = await _priceListDao.GetManyAsync(filter, new SortParams(), true);

            foreach (var priceList in pricelists.Entities)
            {
                await _priceListDao.SetStatusAsync(priceList.Id, PriceListStatus.Processing);

                try
                {
                    await ParcePriceList(priceList.Id);
                }
                catch (Exception)
                {
                    await _priceListDao.SetStatusAsync(priceList.Id, PriceListStatus.Failed);
                }
            }
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemSource> items,
            string priceListId, string supplierId)
        {
            var itemsMatched = _mapper.Map<List<PriceListItemMatched>>(items);

            itemsMatched.ForEach(x =>
            {
                x.Id = _priceListDao.GenerateNewObjectId();
                x.PriceListId = priceListId;
                x.Code = string.IsNullOrWhiteSpace(x.Code) ? null : x.Code;
                x.ProductName = string.IsNullOrWhiteSpace(x.ProductName) ? null : x.ProductName;
            });

            itemsMatched = await _categoryDao.MatchItemsAsync(itemsMatched);
            itemsMatched = await _supplierProductDao.MatchItemsAsync(itemsMatched, supplierId);
            itemsMatched = await _productDao.MatchItemsAsync(itemsMatched);
            itemsMatched = await _searchClient.MatchItemsAsync(itemsMatched);
            itemsMatched = await _categoryPropertyDao.MatchItemsAsync(itemsMatched);
            itemsMatched = await _propertyValueDao.MatchItemsAsync(itemsMatched);
            itemsMatched = await _imageDao.MatchItemsAsync(itemsMatched);

            return itemsMatched;
        }

        public async Task CommitPriceListAsync(string id)
        {
            var priceList = await _priceListDao.GetOneAsync(id, true);

            var filter = new PriceListItemFilter
            {
                PriceListId = id,
                Skip = false
            };

            var itemsResult = await _priceListItemDao.GetManyAsync(filter);
            var items = itemsResult.Entities;

            items = await _productDao.UpdateNamesAsync(items);

            items = await _categoryDao.AddAbsentItemsAsync(items);
            items = await _categoryPropertyDao.AddAbsentItemsAsync(items);
            items = await _propertyValueDao.AddAbsentItemsAsync(items);
            items = await _productService.AddAbsentItemsAsync(items);
            items = await _imageDao.AddAbsentItemsAsync(items);
            await _supplierProductDao.AddAbsentItemsAsync(items, priceList.SupplierId);

            await _priceListDao.SetStatusAsync(priceList.Id, PriceListStatus.Committed);
        }

        public async Task DownloadImages()
        {
            var filter = new GimImageFilter {Status = GimImageDownloadStatus.NotDownloaded};

            var images = await _imageDao.GetManyAsync(filter, 0, 10);

            if (images.Count == 0)
            {
                return;
            }

            filter = new GimImageFilter {Ids = images.Entities.Select(img => img.Id).ToList()};
            await _imageDao.SetStatusManyAsync(filter, GimImageDownloadStatus.Downloading);

            Parallel.ForEach(images.Entities, new ParallelOptions {MaxDegreeOfParallelism = 10}, img =>
            {
                var webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.UserAgent,
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                try
                {
                    img.Data = webClient.DownloadData(img.Url);
                    img.Size = img.Data.Length;
                    img.Status = GimImageDownloadStatus.DownloadSuccess;
                }
                catch (Exception)
                {
                    img.Data = null;
                    img.Status = GimImageDownloadStatus.DownloadFail;
                }
            });

            await _imageDao.ReplaceManyAsync(images.Entities);
        }

        private async Task ParcePriceList(string priceListId)
        {
            var priceList = await _priceListDao.GetOneAsync(priceListId, false);

            var items = new List<PriceListItemSource>();

            var compileResult = priceList.ProcessingRule.RulesSource == RulesSource.Code
                ? _compiler.Compile(priceList.ProcessingRule.Code, Templates.Xlsx)
                : _compiler.Compile(priceList.ProcessingRule.Code);

            if (!compileResult.EmitResult.Success)
            {
            }

            var file = priceList.PriceListFile;
            var ext = Path.GetExtension(file.Name)?.Replace(".", "");

            switch (ext)
            {
                case "xlsx":
                    items = Xlsx.Parse(compileResult.Assembly, file.Data);
                    break;
                case "csv":
                    //FromCsv(file);
                    break;
                default:
                    return;
            }

            var itemsMatched = await MatchItemsAsync(items, priceList.Id, priceList.SupplierId);
            itemsMatched = SetItemsStatuses(itemsMatched);

            await _priceListItemDao.AddManyAsync(itemsMatched);

            await _priceListItemPropertyDao.AddManyAsync(itemsMatched.SelectMany(item => item.Properties).ToList());

            await _priceListItemImageDao.AddManyAsync(itemsMatched.SelectMany(item => item.Images).ToList());

            priceList = SetPriceListStatuses(priceList, itemsMatched);
            priceList.ParsedDate = DateTime.Now;
            await _priceListDao.UpdateOneAsync(priceList);
        }

        private static List<PriceListItemMatched> SetItemsStatuses(IEnumerable<PriceListItemMatched> items)
        {
            var newItems = items.ToList();

            newItems.ForEach(item =>
            {
                item.Status = PriceListItemStatus.Ok;
                item.NameStatus = PriceListItemStatus.Ok;

                // в исходных данных не заполнены необходимые поля
                if (string.IsNullOrWhiteSpace(item.Code) )
                {
                    item.Errors.Add(PriceListItemError.NoCode);
                    item.Status = PriceListItemStatus.Error;
                }

                if (string.IsNullOrWhiteSpace(item.ProductName))
                {
                    item.Errors.Add(PriceListItemError.NoProductName);
                    item.Status = PriceListItemStatus.Error;
                }

                if (item.Price1 == decimal.Zero)
                {
                    item.Errors.Add(PriceListItemError.NoPrice);
                    item.Status = PriceListItemStatus.Error;
                }

                // Если категория не сопоставлена, и это конечная категория в ветке, считаем ошибкой
                if (!string.IsNullOrWhiteSpace(item.Category1Name) && string.IsNullOrWhiteSpace(item.Category1Id) &&
                    string.IsNullOrWhiteSpace(item.Category2Name) && string.IsNullOrWhiteSpace(item.Category3Name) &&
                    string.IsNullOrWhiteSpace(item.Category4Name) && string.IsNullOrWhiteSpace(item.Category5Name))
                {
                    item.Errors.Add(PriceListItemError.CategoryError);
                    item.Status = PriceListItemStatus.Error;
                    item.Category1Status = PriceListItemStatus.Error;
                }

                if (!string.IsNullOrWhiteSpace(item.Category2Name) && string.IsNullOrWhiteSpace(item.Category2Id) &&
                    string.IsNullOrWhiteSpace(item.Category3Name) && string.IsNullOrWhiteSpace(item.Category4Name) &&
                    string.IsNullOrWhiteSpace(item.Category5Name))
                {
                    item.Errors.Add(PriceListItemError.CategoryError);
                    item.Status = PriceListItemStatus.Error;
                    item.Category2Status = PriceListItemStatus.Error;
                }

                if (!string.IsNullOrWhiteSpace(item.Category3Name) && string.IsNullOrWhiteSpace(item.Category3Id) &&
                    string.IsNullOrWhiteSpace(item.Category4Name) && string.IsNullOrWhiteSpace(item.Category5Name))
                {
                    item.Errors.Add(PriceListItemError.CategoryError);
                    item.Status = PriceListItemStatus.Error;
                    item.Category3Status = PriceListItemStatus.Error;
                }

                if (!string.IsNullOrWhiteSpace(item.Category4Name) && string.IsNullOrWhiteSpace(item.Category4Id) &&
                    string.IsNullOrWhiteSpace(item.Category5Name))
                {
                    item.Errors.Add(PriceListItemError.CategoryError);
                    item.Status = PriceListItemStatus.Error;
                    item.Category4Status = PriceListItemStatus.Error;
                }

                if (!string.IsNullOrWhiteSpace(item.Category5Name) && string.IsNullOrWhiteSpace(item.Category5Id))
                {
                    item.Errors.Add(PriceListItemError.CategoryError);
                    item.Status = PriceListItemStatus.Error;
                    item.Category5Status = PriceListItemStatus.Error;
                }

                // товар не был сопоставлен или наименование несоответствует данным в БД
                if (string.IsNullOrWhiteSpace(item.ProductId) || item.ProductName != item.Product.Name)
                {
                    item.Errors.Add(PriceListItemError.PropertiesMismatch);
                    item.Status = PriceListItemStatus.Error;
                    item.NameStatus = PriceListItemStatus.Error;
                }

                // характеристика не была сопоставлена
                //if (item.Properties.Any(p => string.IsNullOrWhiteSpace(p.PropertyId)))
                //{
                //    item.Status = PriceListItemStatus.Error;
                //}

                //var newProduct = string.IsNullOrWhiteSpace(item.ProductId);
                item.Properties.ForEach(property =>
                {
                    property.Status = PriceListItemStatus.Ok;

                    // Характеристика не сопоставлена
                    if (string.IsNullOrWhiteSpace(property.PropertyId))
                    {
                        //if(!newProduct) item.Status = PriceListItemStatus.Error;
                        property.Status = PriceListItemStatus.Error;
                    }

                    var productProperty =
                        item.Product?.Properties.FirstOrDefault(x => x.PropertyId == property.PropertyId);
                    if (productProperty != null && productProperty.Id != property.ValueId)
                    {
                        //if(!newProduct) item.Status = PriceListItemStatus.Error;
                        property.Status = PriceListItemStatus.Error;
                    }
                });
            });

            return newItems;
        }

        private PriceList SetPriceListStatuses(PriceList priceList, IEnumerable<PriceListItemMatched> items)
        {
            var newItems = items.ToList();

            priceList.HasUnprocessedCodeErrors = newItems.Any(x => string.IsNullOrWhiteSpace(x.Code));
            priceList.HasUnprocessedNameErrors = newItems.Any(x => string.IsNullOrWhiteSpace(x.ProductName));
            priceList.HasUnprocessedPriceErrors = newItems.Any(x => x.Price1 == decimal.Zero);


            priceList.HasUnprocessedErrors = newItems.Any(x =>
            {
                var hasCategoryError = _priceListItemDao.HasCategoryError(x);

                return hasCategoryError || priceList.HasUnprocessedCodeErrors || priceList.HasUnprocessedNameErrors ||
                       priceList.HasUnprocessedPriceErrors;
            });

            priceList.HasPropertiesErrors = newItems
                .SelectMany(x => x.Properties)
                .Any(x => x.Status == PriceListItemStatus.Error);

            priceList.Status = newItems.Any(x => x.Status != PriceListItemStatus.Ok)
                ? PriceListStatus.Errors
                : PriceListStatus.Ready;
            priceList.StatusDate = DateTime.Now;

            return priceList;
        }
    }
}
