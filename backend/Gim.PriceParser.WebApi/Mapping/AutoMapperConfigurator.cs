using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Search;
using Gim.PriceParser.Dal.Impl.Mongo.Mapping;
using Gim.PriceParser.WebApi.Models;

namespace Gim.PriceParser.WebApi.Mapping
{
    public static class AutoMapperConfigurator
    {
        public static IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(mce =>
            {
                mce.AddProfile<DalMappingProfile>();
                mce.AddProfile<SearchMappingProfile>();

                mce.CreateMap(typeof(EntityVersion<>), typeof(EntityVersionDto<>));
                mce.CreateMap(typeof(GetAllResult<>), typeof(GetAllResultDto<>));

                mce.AddProfile<CategoryMappingProfile>();
                mce.AddProfile<GimFileMappingProfile>();
                mce.AddProfile<ImageMappingProfile>();
                mce.AddProfile<ManufacturerMappingProfile>();
                mce.AddProfile<PriceListMappingProfile>();
                mce.AddProfile<PriceListItemMappingProfile>();
                mce.AddProfile<ProcessingRuleMappingProfile>();
                mce.AddProfile<ProductMappingProfile>();
                mce.AddProfile<ProductPropertyMappingProfile>();
                mce.AddProfile<ProductPropertyValueMappingProfile>();
                mce.AddProfile<SupplierMappingProfile>();
                mce.AddProfile<SupplierProductMappingProfile>();
                mce.AddProfile<SchedulerTaskMappingProfile>();
                mce.AddProfile<UserMappingProfile>();
                mce.AddProfile<UserRoleMappingProfile>();
            });

            return mappingConfig.CreateMapper();
        }

        public static void Init(IMapperConfigurationExpression mce)
        {
            mce.AddProfile<DalMappingProfile>();

            mce.CreateMap(typeof(EntityVersion<>), typeof(EntityVersionDto<>));
            mce.CreateMap(typeof(GetAllResult<>), typeof(GetAllResultDto<>));

            mce.AddProfile<CategoryMappingProfile>();
            mce.AddProfile<GimFileMappingProfile>();
            mce.AddProfile<ImageMappingProfile>();
            mce.AddProfile<ManufacturerMappingProfile>();
            mce.AddProfile<PriceListMappingProfile>();
            mce.AddProfile<PriceListItemMappingProfile>();
            mce.AddProfile<ProcessingRuleMappingProfile>();
            mce.AddProfile<ProductMappingProfile>();
            mce.AddProfile<ProductPropertyMappingProfile>();
            mce.AddProfile<ProductPropertyValueMappingProfile>();
            mce.AddProfile<SupplierMappingProfile>();
            mce.AddProfile<SupplierProductMappingProfile>();
            mce.AddProfile<SchedulerTaskMappingProfile>();
            mce.AddProfile<UserMappingProfile>();
            mce.AddProfile<UserRoleMappingProfile>();
        }
    }
}