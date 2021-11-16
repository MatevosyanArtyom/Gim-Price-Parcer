using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.ProductProperty;
using Gim.PriceParser.WebApi.Models.ProductPropertyValue;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ProductPropertyValueMappingProfile : Profile
    {
        public ProductPropertyValueMappingProfile()
        {
            CreateMap<ProductPropertyValueBase, CategoryPropertyValue>()
                .IncludeAllDerived()
                .ForMember(dst => dst.Property, opt => opt.Ignore())
                .ForMember(
                    dst => dst.PropertyId,
                    opt => opt.MapFrom(src => src.Property)
                );

            CreateMap<ProductPropertyValueAdd, CategoryPropertyValue>();
            CreateMap<ProductPropertyValueEdit, CategoryPropertyValue>();

            CreateMap<CategoryPropertyValue, ProductPropertyValueEdit>()
                .ForMember(
                    dst => dst.Property,
                    opt => opt.MapFrom(src => src.PropertyId)
                );

            CreateMap<CategoryPropertyValue, ProductPropertyValueLookup>().ForMember(
                dst => dst.Property,
                opt => opt.MapFrom(src => src.Property.Name)
            );
            CreateMap<GetAllResult<CategoryProperty>, GetAllResultDto<ProductPropertyLookup>>();
        }
    }
}