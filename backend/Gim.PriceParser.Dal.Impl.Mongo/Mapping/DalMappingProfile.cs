using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Gim.PriceParser.Bll.Common;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.EntityVersion;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceList;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole;
using MongoDB.Bson;
using MongoDB.Driver;
using SortDirection = Gim.PriceParser.Bll.Common.Sort.SortDirection;

namespace Gim.PriceParser.Dal.Impl.Mongo.Mapping
{
    public class DalMappingProfile : Profile
    {
        public DalMappingProfile()
        {
            CreateMap<string, ObjectId>()
                .ConvertUsing(src =>
                    string.IsNullOrWhiteSpace(src) ? ObjectId.Empty : ObjectId.Parse(src));

            CreateMap<ObjectId, string>().ConvertUsing(src => src == ObjectId.Empty ? "" : src.ToString());

            CreateMap(typeof(EntityVersionDo<>), typeof(EntityVersion<>));

            // MongoDb.Lookup делает коллекцию присоединенных элементов, даже если присоединяешь по Id и там не более одного элемента всегда.
            // Поэтому при маппинге ClassnameFullDo->Classname берем первый элемент

            CreateCategoryMaps();
            CreateCategoryPropertyMaps();
            CreateCategoryPropertyValueMaps();
            CreateImageMaps();
            CreateManufacturerMaps();
            CreatePriceListMaps();
            CreatePriceListItemMaps();
            CreatePriceListItemImageMaps();
            CreatePriceListItemPropertyMaps();
            CreateProcessingRuleMaps();
            CreateProductMaps();
            CreateSupplierMaps();
            CreateSupplierProductMaps();
            CreateSchedulerTaskMaps();
            CreateUserMaps();
            CreateUserRoleMaps();
        }

        private void CreateCategoryMaps()
        {
            CreateMap<CategoryFilter, FilterDefinition<CategoryDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<CategoryDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<CategoryDo>> {FilterDefinition<CategoryDo>.Empty};

                    if (filter.Ids != null && filter.Ids.Any())
                    {
                        var objIds = context.Mapper.Map<List<ObjectId>>(filter.Ids);
                        filterDefinitions.Add(buildersFilter.In(x => x.Id, objIds));
                    }

                    // Здесь null означает отсутствие фильтрации по родителям. Необходимо в методах работы с номенклатурой
                    if (filter.Parents != null && filter.Parents.Any() || filter.IncludeRoot)
                    {
                        var objIds = new List<ObjectId>();
                        if (filter.Parents != null)
                        {
                            objIds = context.Mapper.Map<List<ObjectId>>(filter.Parents);
                        }

                        if (filter.IncludeRoot)
                        {
                            objIds.Add(ObjectId.Empty);
                        }

                        filterDefinitions.Add(buildersFilter.In(x => x.ParentId, objIds));
                    }

                    if (filter.Names != null && filter.Names.Any())
                    {
                        var fieldNameOf = $"{nameof(CategoryDo.Mappings)}.{nameof(CategoryMappingItem.Name)}";
                        var filterDo = Builders<CategoryDo>.Filter.Or(buildersFilter.In(x => x.Name, filter.Names),
                            Builders<CategoryDo>.Filter.In(fieldNameOf, filter.Names));
                        filterDefinitions.Add(filterDo);
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<Category, CategoryDo>()
                .ForMember(
                    dst => dst.Version,
                    mce => mce.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Version)
                            ? ObjectId.GenerateNewId()
                            : ObjectId.Parse(src.Version)));

            CreateMap<CategoryDo, Category>()
                .IncludeAllDerived()
                .ForMember(
                    dst => dst.Ancestors,
                    mce => mce.MapFrom(
                        src => (src.Path ?? "").Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries)))
                .ForMember(
                    dst => dst.Modified,
                    mce => mce.MapFrom(src => src.Version.CreationTime));

            CreateMap<CategoryDo, CategoryVersionDo>()
                .ForMember(
                    dst => dst.Entity,
                    mce => mce.MapFrom(src => src));


            CreateMap<CategoryFullDo, Category>()
                .ForMember(
                    dst => dst.HasChildren,
                    mce => mce.MapFrom(src => src.Children.Any()))
                .ForMember(
                    dst => dst.Parent,
                    mce => mce.MapFrom(src => src.Parents.FirstOrDefault())
                )
                .ForMember(
                    dst => dst.ProductsCount,
                    mce => mce.MapFrom(src => src.Products.Count())
                );

            CreateMap<CategoryVersionDo, CategoryDo>()
                .ConstructUsing((src, ctx) => ctx.Mapper.Map<CategoryDo>(src.Entity));

            CreateMap<CategoryVersionFullDo, Category>()
                .ConstructUsing((src, ctx) => ctx.Mapper.Map<Category>(src.Entity));
        }

        private void CreateCategoryPropertyMaps()
        {
            CreateMap<CategoryPropertyFilter, FilterDefinition<CategoryPropertyDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<CategoryPropertyDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<CategoryPropertyDo>>
                        {FilterDefinition<CategoryPropertyDo>.Empty};

                    if (!string.IsNullOrWhiteSpace(filter.CategoryId))
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.CategoryId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<CategoryProperty, CategoryPropertyDo>();

            CreateMap<CategoryPropertyDo, CategoryProperty>();

            CreateMap<CategoryPropertyFullDo, CategoryProperty>()
                .ForMember(
                    dst => dst.Category,
                    mce => mce.MapFrom(src => src.Categories.FirstOrDefault())
                );
        }

        private void CreateCategoryPropertyValueMaps()
        {
            CreateMap<CategoryPropertyValueFilter, FilterDefinition<CategoryPropertyValueDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<CategoryPropertyValueDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<CategoryPropertyValueDo>>
                        {FilterDefinition<CategoryPropertyValueDo>.Empty};

                    if (filter.ValuesIds != null)
                    {
                        var valuesObjIds = context.Mapper.Map<List<ObjectId>>(filter.ValuesIds);
                        filterDefinitions.Add(buildersFilter.In(x => x.Id, valuesObjIds));
                    }

                    if (filter.PropertiesIds != null)
                    {
                        var propertiesObjIds = context.Mapper.Map<List<ObjectId>>(filter.PropertiesIds);
                        filterDefinitions.Add(buildersFilter.In(x => x.PropertyId, propertiesObjIds));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.PropertyId))
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.PropertyId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.PropertyId, objId));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<ObjectId, CategoryPropertyValue>()
                .ConvertUsing((src, dst, ctx) => new CategoryPropertyValue {Id = ctx.Mapper.Map<string>(src)});

            CreateMap<CategoryPropertyValue, ObjectId>()
                .ConvertUsing((src, dst, ctx) => ctx.Mapper.Map<ObjectId>(src.Id));

            CreateMap<PriceListItemPropertyMatched, CategoryPropertyValueDo>()
                .ForMember(
                    dst => dst.Id,
                    opt => opt.MapFrom(src => src.ValueId))
                .ForMember(
                    dst => dst.Name,
                    opt => opt.MapFrom(src => src.PropertyValue));

            CreateMap<CategoryPropertyValue, CategoryPropertyValueDo>();

            CreateMap<CategoryPropertyValueDo, CategoryPropertyValue>();

            CreateMap<CategoryPropertyValueFullDo, CategoryPropertyValue>()
                .ForMember(
                    dst => dst.Property,
                    mce => mce.MapFrom(src => src.Properties.FirstOrDefault())
                );
        }

        private void CreateImageMaps()
        {
            CreateMap<GimImageFilter, FilterDefinition<ImageDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<ImageDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<ImageDo>>
                    {
                        FilterDefinition<ImageDo>.Empty
                    };

                    if (filter.Ids != null && filter.Ids.Any())
                    {
                        var idsObj = context.Mapper.Map<List<ObjectId>>(filter.Ids);
                        filterDefinitions.Add(buildersFilter.In(x => x.Id, idsObj));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.ProductId))
                    {
                        var productObjId = context.Mapper.Map<ObjectId>(filter.ProductId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.ProductId, productObjId));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<GimImage, ImageDo>().ReverseMap();
        }

        private void CreateManufacturerMaps()
        {
            CreateMap<ManufacturerDo, Manufacturer>().ReverseMap();
        }

        private void CreatePriceListMaps()
        {
            CreateMap<PriceListFilter, FilterDefinition<PriceListDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<PriceListDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<PriceListDo>>
                    {
                        FilterDefinition<PriceListDo>.Empty
                    };

                    if (filter.SeqId != null && filter.SeqId > 0)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    if (filter.ParsedFrom != null)
                    {
                        filterDefinitions.Add(buildersFilter.Gte(x => x.ParsedDate, filter.ParsedFrom));
                    }

                    if (filter.ParsedTo != null)
                    {
                        filterDefinitions.Add(buildersFilter.Lte(x => x.ParsedDate, filter.ParsedTo));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.SupplierId))
                    {
                        var supplierIdObj = context.Mapper.Map<ObjectId>(filter.SupplierId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SupplierId, supplierIdObj));
                    }

                    if (filter.RulesSource != null)
                    {
                        //filterDefinitions.Add(buildersFilter.Eq(x => x.RulesSource, filter.RulesSource));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    if (filter.ExceptStatus != null)
                    {
                        var filterDo = buildersFilter.Eq(x => x.Status, filter.ExceptStatus);
                        filterDefinitions.Add(buildersFilter.Not(filterDo));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<PriceListDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<PriceListDo>.Sort;

                    Expression<Func<PriceListDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy)
                    {
                        case "id":
                        case "seqId":
                            field = x => x.Id;
                            break;
                        case "parsedDate":
                            field = x => x.ParsedDate;
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<PriceList, PriceListDo>();

            CreateMap<PriceListDo, PriceList>();

            CreateMap<PriceListFullDo, PriceList>()
                .ForMember(
                    dst => dst.Supplier,
                    mce => mce.MapFrom(src => src.Suppliers.FirstOrDefault()))
                .ForMember(
                    dst => dst.SchedulerTask,
                    mce => mce.MapFrom(src => src.SchedulerTasks.FirstOrDefault()))
                .ForMember(
                    dst => dst.ProcessingRule,
                    mce => mce.MapFrom(src => src.ProcessingRules.FirstOrDefault()))
                .ForMember(
                    dst => dst.Author,
                    mce => mce.MapFrom(src => src.Authors.FirstOrDefault()));
        }

        private void CreatePriceListItemMaps()
        {
            CreateMap<PriceListItemFilter, FilterDefinition<PriceListItemDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<PriceListItemDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<PriceListItemDo>>
                        {FilterDefinition<PriceListItemDo>.Empty};

                    if (!string.IsNullOrWhiteSpace(filter.PriceListId))
                    {
                        var priceListIdObj = context.Mapper.Map<ObjectId>(filter.PriceListId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.PriceListId, priceListIdObj));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Code))
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Code, filter.Code));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                    {
                        var filterDo = buildersFilter.Or(buildersFilter.Eq(x => x.Category1Name, filter.CategoryName),
                            buildersFilter.Eq(x => x.Category2Name, filter.CategoryName),
                            buildersFilter.Eq(x => x.Category3Name, filter.CategoryName),
                            buildersFilter.Eq(x => x.Category4Name, filter.CategoryName),
                            buildersFilter.Eq(x => x.Category5Name, filter.CategoryName));

                        filterDefinitions.Add(filterDo);
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category1Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Category1Name,
                            $"/.*{filter.Category1Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category2Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Category2Name,
                            $"/.*{filter.Category2Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category3Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Category3Name,
                            $"/.*{filter.Category3Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category4Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Category4Name,
                            $"/.*{filter.Category4Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category5Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Category5Name,
                            $"/.*{filter.Category5Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.ProductNameEq))
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.ProductName, filter.ProductNameEq));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.ProductNameRegEx))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.ProductName,
                            $"/.*{filter.ProductNameRegEx}.*/i"));
                    }

                    if (filter.Price1.HasValue)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Price1, filter.Price1));
                    }

                    if (filter.Price2.HasValue)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Price2, filter.Price2));
                    }

                    if (filter.Price3.HasValue)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Price3, filter.Price3));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    if (filter.Skip != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Skip, filter.Skip));
                    }

                    if (filter.UnprocessedItemsOnly ?? false)
                    {
                        var nameFilter =
                            buildersFilter.And(buildersFilter.Eq(x => x.NameStatus, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.NameAction, PriceListItemAction.Unknown));

                        // элемент не обработан, если категория не сопоставлена, не задан маппинг и строка не пропущена
                        var category1Filter = buildersFilter.And(
                            buildersFilter.Eq(x => x.Category1Status, PriceListItemStatus.Error),
                            buildersFilter.Eq(x => x.Category1Action, PriceListItemCategoryAction.Unknown),
                            buildersFilter.Eq(x => x.MapTo1Id, ObjectId.Empty));

                        var category2Filter = buildersFilter.And(
                            buildersFilter.Eq(x => x.Category2Status, PriceListItemStatus.Error),
                            buildersFilter.Eq(x => x.Category2Action, PriceListItemCategoryAction.Unknown),
                            buildersFilter.Eq(x => x.MapTo2Id, ObjectId.Empty));

                        var category3Filter = buildersFilter.And(
                            buildersFilter.Eq(x => x.Category3Status, PriceListItemStatus.Error),
                            buildersFilter.Eq(x => x.Category3Action, PriceListItemCategoryAction.Unknown),
                            buildersFilter.Eq(x => x.MapTo3Id, ObjectId.Empty));

                        var category4Filter = buildersFilter.And(
                            buildersFilter.Eq(x => x.Category4Status, PriceListItemStatus.Error),
                            buildersFilter.Eq(x => x.Category4Action, PriceListItemCategoryAction.Unknown),
                            buildersFilter.Eq(x => x.MapTo4Id, ObjectId.Empty));

                        var category5Filter = buildersFilter.And(
                            buildersFilter.Eq(x => x.Category5Status, PriceListItemStatus.Error),
                            buildersFilter.Eq(x => x.Category5Action, PriceListItemCategoryAction.Unknown),
                            buildersFilter.Eq(x => x.MapTo5Id, ObjectId.Empty));

                        // Фильтр элементов, у которых есть ошибка и не определено действие
                        var errorItemsFilter = buildersFilter.Or(nameFilter, category1Filter, category2Filter,
                            category3Filter, category4Filter, category5Filter);

                        filterDefinitions.Add(errorItemsFilter);

                        //Фильтр непропущенных с ошибками
                        //var notSkippedItems =
                        //    buildersFilter.And(errorItemsFilter, buildersFilter.Eq(x => x.Skip, false));

                        filterDefinitions.Add(buildersFilter.Eq(x => x.Skip, false));
                    }

                    if (filter.ProcessedItemsOnly ?? false)
                    {
                        // фильтр элементов, у которых была ошибка наименования и действие установлено
                        // либо ошибки не было 
                        var nameFilter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.NameStatus, PriceListItemStatus.Error),
                                buildersFilter.Not(buildersFilter.Eq(x => x.NameAction, PriceListItemAction.Unknown))),
                            buildersFilter.Eq(x => x.NameStatus, PriceListItemStatus.Ok));

                        // фильтр элементов, у которых была ошибка категории и задан маппинг
                        // либо ошибки не было
                        var category1Filter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.Category1Status, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.Category1Action, PriceListItemCategoryAction.MapTo)),
                            buildersFilter.Eq(x => x.Category1Status, PriceListItemStatus.Unknown));

                        var category2Filter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.Category2Status, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.Category2Action, PriceListItemCategoryAction.MapTo)),
                            buildersFilter.Eq(x => x.Category2Status, PriceListItemStatus.Unknown));

                        var category3Filter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.Category3Status, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.Category3Action, PriceListItemCategoryAction.MapTo)),
                            buildersFilter.Eq(x => x.Category3Status, PriceListItemStatus.Unknown));

                        var category4Filter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.Category4Status, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.Category4Action, PriceListItemCategoryAction.MapTo)),
                            buildersFilter.Eq(x => x.Category4Status, PriceListItemStatus.Unknown));

                        var category5Filter = buildersFilter.Or(
                            buildersFilter.And(buildersFilter.Eq(x => x.Category5Status, PriceListItemStatus.Error),
                                buildersFilter.Eq(x => x.Category5Action, PriceListItemCategoryAction.MapTo)),
                            buildersFilter.Eq(x => x.Category5Status, PriceListItemStatus.Unknown));

                        var errorFilter = buildersFilter.Eq(x => x.Status, PriceListItemStatus.Error);

                        // отбор элементов, у которых статус Error, но ошибки обработаны либо их не было
                        // такая ситуация означает, что хотя бы 1 ошибка гарантированно была и она исправлена
                        // такому элементу нужно установить статус Fixed
                        var processedFilter = buildersFilter.And(nameFilter, category1Filter, category2Filter,
                            category3Filter, category4Filter, category5Filter, errorFilter);
                        filterDefinitions.Add(processedFilter);
                    }

                    if (filter.UnprocessedCodeError ?? false)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Code, null));
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Skip, false));
                    }

                    if (filter.UnprocessedNameErrors ?? false)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.ProductName, null));
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Skip, false));
                    }

                    if (filter.UnprocessedPriceError ?? false)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Price1, decimal.Zero));
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Skip, false));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<PriceListItemDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<PriceListItemDo>.Sort;

                    Expression<Func<PriceListItemDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    //switch (sort.SortBy)
                    //{
                    //    case "fullname":
                    //        field = x => x.Fullname;
                    //        break;
                    //    case "createdDate":
                    //        field = x => x.CreatedDate;
                    //        break;
                    //    case "email":
                    //        field = x => x.Email;
                    //        break;
                    //}

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<ProductSynonym, ProductSynonymDo>().ReverseMap();

            CreateMap<PriceListItemSource, PriceListItemMatched>()
                .ForMember(
                    dst => dst.Images,
                    opt => opt.MapFrom(src => src.Images));

            CreateMap<PriceListItemMatched, PriceListItemDo>().ReverseMap();
            CreateMap<PriceListItemFullDo, PriceListItemMatched>()
                .ForMember(
                    dst => dst.SupplierProduct,
                    opt => opt.MapFrom(src => src.SupplierProducts.FirstOrDefault()))
                .ForMember(
                    dst => dst.Category1,
                    opt => opt.MapFrom(src => src.Categories1.FirstOrDefault()))
                .ForMember(
                    dst => dst.Category2,
                    opt => opt.MapFrom(src => src.Categories2.FirstOrDefault()))
                .ForMember(
                    dst => dst.Category3,
                    opt => opt.MapFrom(src => src.Categories3.FirstOrDefault()))
                .ForMember(
                    dst => dst.Category4,
                    opt => opt.MapFrom(src => src.Categories4.FirstOrDefault()))
                .ForMember(
                    dst => dst.Category5,
                    opt => opt.MapFrom(src => src.Categories5.FirstOrDefault()))
                .ForMember(
                    dst => dst.Product,
                    opt => opt.MapFrom(src => src.Products.FirstOrDefault()));
        }

        private void CreatePriceListItemImageMaps()
        {
            CreateMap<PriceListItemImageFilter, FilterDefinition<PriceListItemImageDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<PriceListItemImageDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<PriceListItemImageDo>>
                        {FilterDefinition<PriceListItemImageDo>.Empty};

                    if (filter.PriceListItemsIds != null && filter.PriceListItemsIds.Any())
                    {
                        var priceListItemsIdsObj = context.Mapper.Map<List<ObjectId>>(filter.PriceListItemsIds);
                        filterDefinitions.Add(buildersFilter.In(x => x.PriceListItemId, priceListItemsIdsObj));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<string, PriceListItemImage>()
                .ConstructUsing(src => new PriceListItemImage
                {
                    Url = src
                });

            CreateMap<PriceListItemImage, PriceListItemImageDo>();

            CreateMap<PriceListItemImageDo, PriceListItemImage>();

            CreateMap<PriceListItemImageFullDo, PriceListItemImage>()
                .ForMember(
                    dst => dst.Image,
                    opt => opt.MapFrom(src => src.GimImages.FirstOrDefault()));
        }

        private void CreatePriceListItemPropertyMaps()
        {
            CreateMap<PriceListItemPropertyFilter, FilterDefinition<PriceListItemPropertyDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<PriceListItemPropertyDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<PriceListItemPropertyDo>>
                        {FilterDefinition<PriceListItemPropertyDo>.Empty};

                    if (!string.IsNullOrWhiteSpace(filter.PriceListItemId))
                    {
                        var priceListItemIdObj = context.Mapper.Map<ObjectId>(filter.PriceListItemId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.PriceListItemId, priceListItemIdObj));
                    }

                    if (filter.PriceListItemsIds != null && filter.PriceListItemsIds.Any())
                    {
                        var priceListItemsIdsObj = context.Mapper.Map<List<ObjectId>>(filter.PriceListItemsIds);
                        filterDefinitions.Add(buildersFilter.In(x => x.PriceListItemId, priceListItemsIdsObj));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.PropertyKey))
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.PropertyKey, filter.PropertyKey));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<KeyValuePair<string, string>, PriceListItemPropertyMatched>()
                .ForMember(dst => dst.Value, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    var result = new PriceListItemPropertyMatched
                    {
                        PropertyKey = src.Key,
                        PropertyValue = src.Value
                    };
                    return result;
                });

            CreateMap<PriceListItemPropertyMatched, PriceListItemPropertyDo>().ReverseMap();
            CreateMap<PriceListItemPropertyFullDo, PriceListItemPropertyMatched>()
                .ForMember(
                    dst => dst.Property,
                    opt => opt.MapFrom(src => src.Properties.FirstOrDefault()))
                .ForMember(
                    dst => dst.Value,
                    opt => opt.MapFrom(src => src.Values.FirstOrDefault()));
        }

        private void CreateProcessingRuleMaps()
        {
            CreateMap<ProcessingRuleFilter, FilterDefinition<ProcessingRuleDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<ProcessingRuleDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<ProcessingRuleDo>>
                        {FilterDefinition<ProcessingRuleDo>.Empty};

                    if (filter.ArchivedFilter != null)
                    {
                        switch (filter.ArchivedFilter)
                        {
                            case ArchivedFilter.OnlyActive:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, false));
                                break;
                            case ArchivedFilter.OnlyArchived:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, true));
                                break;
                        }
                    }

                    if (filter.SeqId != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Name, $"/.*{filter.Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.SupplierId))
                    {
                        var supplierObjId = context.Mapper.Map<ObjectId>(filter.SupplierId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SupplierId, supplierObjId));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<ProcessingRuleDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<ProcessingRuleDo>.Sort;

                    Expression<Func<ProcessingRuleDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy)
                    {
                        case "id":
                        case "seqId":
                            field = x => x.Id;
                            break;
                        case "name":
                            field = x => x.Name;
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<ProcessingRuleDo, ProcessingRule>().ReverseMap();
            CreateMap<ProcessingRuleFullDo, ProcessingRule>()
                .ForMember(
                    dst => dst.Supplier,
                    mce => mce.MapFrom(src => src.Suppliers.FirstOrDefault()));
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductFilter, FilterDefinition<ProductDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<ProductDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<ProductDo>> {FilterDefinition<ProductDo>.Empty};

                    if (filter.Ids != null && filter.Ids.Any())
                    {
                        var objIds = context.Mapper.Map<List<ObjectId>>(filter.Ids);
                        filterDefinitions.Add(buildersFilter.In(x => x.Id, objIds));
                    }

                    if (filter.SeqId != null && filter.SeqId > 0)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    var categoryFilterAdded = false;
                    if (!string.IsNullOrWhiteSpace(filter.Category5))
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.Category5);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                        categoryFilterAdded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category4) && !categoryFilterAdded)
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.Category4);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                        categoryFilterAdded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category3) && !categoryFilterAdded)
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.Category3);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                        categoryFilterAdded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category2) && !categoryFilterAdded)
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.Category2);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                        categoryFilterAdded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category1) && !categoryFilterAdded)
                    {
                        var objId = context.Mapper.Map<ObjectId>(filter.Category1);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.CategoryId, objId));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Name, $"/.*{filter.Name}.*/i"));
                    }

                    if (filter.Names != null && filter.Names.Any())
                    {
                        filterDefinitions.Add(buildersFilter.In(x => x.Name, filter.Names));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<ProductDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<ProductDo>.Sort;

                    Expression<Func<ProductDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy.ToLower())
                    {
                        case "name":
                            field = x => x.Name;
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<ProductPropertyItem, ObjectId>()
                .ConvertUsing((src, dst, ctx) => ctx.Mapper.Map<ObjectId>(src.ValueId));

            CreateMap<ObjectId, ProductPropertyItem>()
                .ConvertUsing((src, dst, ctx) => new ProductPropertyItem {ValueId = ctx.Mapper.Map<string>(src)});

            CreateMap<Product, ProductDo>()
                .ForMember(
                    dst => dst.Version,
                    mce => mce.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Version)
                            ? ObjectId.GenerateNewId()
                            : ObjectId.Parse(src.Version)));

            CreateMap<ProductDo, Product>();

            CreateMap<Product, ProductVersionDo>();

            CreateMap<ProductVersionDo, ProductDo>()
                .ConvertUsing(src => src.Entity);

            CreateMap<ProductDo, ProductVersionDo>()
                .ForMember(
                    dst => dst.Id,
                    mce => mce.Ignore())
                .ForMember(
                    dst => dst.Entity,
                    mce => mce.MapFrom(src => src));

            CreateMap<ProductFullDo, Product>()
                .ForMember(
                    dst => dst.Category,
                    mce => mce.MapFrom(src => src.Categories.FirstOrDefault())
                )
                .ForMember(
                    dst => dst.Supplier,
                    mce => mce.MapFrom(src => src.Suppliers.FirstOrDefault())
                )
                .ForMember(
                    dst => dst.PriceFrom,
                    mce => mce.MapFrom(src => src.SupplierProducts
                        .SelectMany(x => new List<decimal?> {x.Price1, x.Price2, x.Price3})
                        .Where(x => x.HasValue && x.Value > 0)
                        .Min()))
                .ForMember(
                    dst => dst.SupplierCount,
                    mce => mce.MapFrom(src => src.SupplierProducts.Select(x => x.SupplierId).Distinct().Count()))
                .ForMember(
                    dst => dst.ImageTotalCount,
                    mce => mce.MapFrom(src => src.Images.Count))
                .ForMember(
                    dst => dst.ImagePublishedCount,
                    mce => mce.MapFrom(src => src.Images.Count(x => x.IsPublished)));

            CreateMap<ProductVersionFullDo, Product>()
                .ConvertUsing((src, dst, context) => context.Mapper.Map<Product>(src.Entity));
        }

        private void CreateSupplierMaps()
        {
            CreateMap<SupplierFilter, FilterDefinition<SupplierDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<SupplierDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<SupplierDo>> {FilterDefinition<SupplierDo>.Empty};

                    if (filter.IsArchived != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, filter.IsArchived));
                    }

                    if (filter.SeqId != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    if (filter.CreatedFrom != null)
                    {
                        filterDefinitions.Add(buildersFilter.Gte(x => x.CreatedDate, filter.CreatedFrom));
                    }

                    if (filter.CreatedTo != null)
                    {
                        filterDefinitions.Add(buildersFilter.Lte(x => x.CreatedDate, filter.CreatedTo));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Name, $"/.*{filter.Name}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.City))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.City.Value, $"/.*{filter.City}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Inn))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Inn, $"/.*{filter.Inn}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.UserId))
                    {
                        var userObjId = context.Mapper.Map<ObjectId>(filter.UserId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.UserId, userObjId));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<SupplierDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<SupplierDo>.Sort;

                    Expression<Func<SupplierDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy)
                    {
                        case "createdDate":
                            field = x => x.CreatedDate;
                            break;
                        case "name":
                            field = x => x.Name;
                            break;
                        case "city":
                            field = x => x.City.Value;
                            break;
                        case "inn":
                            field = x => x.Inn;
                            break;
                        case "status":
                            field = x => x.Status;
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<ContactPersonDo, ContactPerson>().ReverseMap();
            CreateMap<BankDetailsDo, BankDetails>().ReverseMap();

            // При добавлении / обновлении - первый этап
            CreateMap<Supplier, SupplierDo>()
                .ForMember(
                    dst => dst.Version,
                    mce => mce.MapFrom(src => ObjectId.GenerateNewId()));

            // При добавлении / обновлении - второй этап
            CreateMap<SupplierDo, SupplierVersionDo>();

            // При добавлении / обновлении - третий этап
            CreateMap<SupplierVersionDo, EntityVersionDo<SupplierVersionDo>>()
                .ForMember(
                    dst => dst.Id,
                    opt => opt.MapFrom(src => src.Version))
                .ForMember(
                    dst => dst.Entity,
                    opt => opt.MapFrom(src => src))
                .ForMember(
                    dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<SupplierDo, Supplier>();

            CreateMap<EntityVersionDo<SupplierVersionDo>, SupplierDo>()
                .ConvertUsing(src => src.Entity);

            CreateMap<EntityVersionFullDo<SupplierVersionDo>, EntityVersion<Supplier>>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.Users.FirstOrDefault()));

            CreateMap<Supplier, SupplierVersionDo>();

            CreateMap<SupplierVersionDo, SupplierDo>();


            CreateMap<SupplierFullDo, Supplier>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.Users.FirstOrDefault()));

            CreateMap<SupplierVersionFullDo, Supplier>()
                .ConvertUsing((src, dst, context) => context.Mapper.Map<Supplier>(src.Entity));
        }

        private void CreateSupplierProductMaps()
        {
            CreateMap<SupplierProductFilter, FilterDefinition<SupplierProductDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<SupplierProductDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<SupplierProductDo>>
                        {FilterDefinition<SupplierProductDo>.Empty};

                    if (!string.IsNullOrWhiteSpace(filter.ProductId))
                    {
                        var productObjId = context.Mapper.Map<ObjectId>(filter.ProductId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.ProductId, productObjId));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Code))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Code, $"/.*{filter.Code}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Name, $"/.*{filter.Name}.*/i"));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SupplierProduct, SupplierProductDo>()
                .ForMember(
                    dst => dst.Version,
                    mce => mce.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Version)
                            ? ObjectId.GenerateNewId()
                            : ObjectId.Parse(src.Version)));

            CreateMap<SupplierProductDo, SupplierProduct>();

            CreateMap<SupplierProductFullDo, SupplierProduct>()
                .ForMember(
                    dst => dst.Supplier,
                    mce => mce.MapFrom(src => src.Suppliers.FirstOrDefault())
                )
                .ForMember(
                    dst => dst.Product,
                    mce => mce.MapFrom(src => src.Products.FirstOrDefault()));
        }

        private void CreateSchedulerTaskMaps()
        {
            CreateMap<SchedulerTaskFilter, FilterDefinition<SchedulerTaskDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<SchedulerTaskDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<SchedulerTaskDo>>
                    {
                        FilterDefinition<SchedulerTaskDo>.Empty
                    };

                    if (filter.StartBy.HasValue)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.StartBy, filter.StartBy));
                    }

                    if (filter.Status.HasValue)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SchedulerTask, SchedulerTaskDo>()
                .ForMember(
                    dst => dst.Version,
                    mce => mce.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Version)
                            ? ObjectId.GenerateNewId()
                            : ObjectId.Parse(src.Version)));

            CreateMap<SchedulerTaskDo, SchedulerTask>()
                .ForMember(
                    dst => dst.Modified,
                    mce => mce.MapFrom(src => src.Version.CreationTime));

            CreateMap<SchedulerTaskFullDo, SchedulerTask>()
                .ForMember(
                    dst => dst.Supplier,
                    mce => mce.MapFrom(src => src.Suppliers.FirstOrDefault())
                );

            CreateMap<SupplierDo, EntityVersionDo<SupplierDo>>()
                .ForMember(
                    dst => dst.Entity,
                    opt => opt.MapFrom(src => src));

            CreateMap<EntityVersionFullDo<SupplierDo>, EntityVersion<Supplier>>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.Users.FirstOrDefault()));
        }

        private void CreateUserMaps()
        {
            CreateMap<GimUserFilter, FilterDefinition<UserDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<UserDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<UserDo>> {FilterDefinition<UserDo>.Empty};

                    if (filter.ArchivedFilter != null)
                    {
                        switch (filter.ArchivedFilter)
                        {
                            case ArchivedFilter.OnlyActive:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, false));
                                break;
                            case ArchivedFilter.OnlyArchived:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, true));
                                break;
                        }
                    }

                    if (filter.SeqId != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    if (filter.CreatedFrom != null)
                    {
                        filterDefinitions.Add(buildersFilter.Gte(x => x.CreatedDate, filter.CreatedFrom));
                    }

                    if (filter.CreatedTo != null)
                    {
                        filterDefinitions.Add(buildersFilter.Lte(x => x.CreatedDate, filter.CreatedTo));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Fullname))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Fullname, $"/.*{filter.Fullname}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Email))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Email, $"/.*{filter.Email}.*/i"));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.RoleId))
                    {
                        var roleIdObj = context.Mapper.Map<ObjectId>(filter.RoleId);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.RoleId, roleIdObj));
                    }

                    if (filter.Status != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.Status, filter.Status));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Token))
                    {
                        var tokenObj = context.Mapper.Map<ObjectId>(filter.Token);
                        filterDefinitions.Add(buildersFilter.Eq(x => x.ChangePasswordToken, tokenObj));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<UserDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<UserDo>.Sort;

                    Expression<Func<UserDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy)
                    {
                        case "fullname":
                            field = x => x.Fullname;
                            break;
                        case "createdDate":
                            field = x => x.CreatedDate;
                            break;
                        case "email":
                            field = x => x.Email;
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<GimUser, UserDo>().ReverseMap();

            CreateMap<UserFullDo, GimUser>()
                .ForMember(
                    dst => dst.Role,
                    opt => opt.MapFrom(src => src.Roles.FirstOrDefault()));
        }

        private void CreateUserRoleMaps()
        {
            CreateMap<GimUserRoleFilter, FilterDefinition<UserRoleFullDo>>()
                .ConstructUsing((filter, context) =>
                {
                    var buildersFilter = Builders<UserRoleFullDo>.Filter;
                    var filterDefinitions = new List<FilterDefinition<UserRoleFullDo>>
                        {FilterDefinition<UserRoleFullDo>.Empty};

                    if (filter.ArchivedFilter != null)
                    {
                        switch (filter.ArchivedFilter)
                        {
                            case ArchivedFilter.OnlyActive:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, false));
                                break;
                            case ArchivedFilter.OnlyArchived:
                                filterDefinitions.Add(buildersFilter.Eq(x => x.IsArchived, true));
                                break;
                        }
                    }

                    if (filter.SeqId != null)
                    {
                        filterDefinitions.Add(buildersFilter.Eq(x => x.SeqId, filter.SeqId));
                    }

                    if (filter.CreatedFrom != null)
                    {
                        filterDefinitions.Add(buildersFilter.Gte(x => x.CreatedDate, filter.CreatedFrom));
                    }

                    if (filter.CreatedTo != null)
                    {
                        filterDefinitions.Add(buildersFilter.Lte(x => x.CreatedDate, filter.CreatedTo));
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Name))
                    {
                        filterDefinitions.Add(buildersFilter.Regex(x => x.Name, $"/.*{filter.Name}.*/i"));
                    }

                    if (filter.UsersFrom != null)
                    {
                        // TODO
                        //filterDefinitions.Add(buildersFilter.Gte(x => x.UsersCount, filter.UsersFrom));
                    }

                    var resultFilter = buildersFilter.And(filterDefinitions);
                    return resultFilter;
                });

            CreateMap<SortParams, SortDefinition<UserRoleFullDo>>()
                .ConstructUsing((sort, context) =>
                {
                    var sortBuilder = Builders<UserRoleFullDo>.Sort;

                    Expression<Func<UserRoleFullDo, object>> field = x => x.Id;
                    var sortDefinition = sortBuilder.Ascending(field);

                    if (string.IsNullOrWhiteSpace(sort.SortBy))
                    {
                        return sortDefinition;
                    }

                    switch (sort.SortBy)
                    {
                        case "name":
                            field = x => x.Name;
                            break;
                        case "createdDate":
                            field = x => x.CreatedDate;
                            break;
                        case "usersCount":
                            field = x => x.Users.Count();
                            break;
                    }

                    switch (sort.SortDirection)
                    {
                        case SortDirection.Asc:
                            sortDefinition = sortBuilder.Ascending(field);
                            break;
                        case SortDirection.Desc:
                            sortDefinition = sortBuilder.Descending(field);
                            break;
                    }

                    return sortDefinition;
                });

            CreateMap<GimUserRole, UserRoleDo>().ReverseMap();

            CreateMap<UserRoleFullDo, GimUserRole>()
                .ForMember(
                    dst => dst.UsersCount,
                    opt => opt.MapFrom(src => src.Users.Count()));
        }
    }
}