using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.ProductProperty;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ProductPropertyMappingProfile : Profile
    {
        public ProductPropertyMappingProfile()
        {
            CreateMap<ProductPropertyBase, CategoryProperty>()
                .IncludeAllDerived()
                .ForMember(dst => dst.Category, opt => opt.Ignore())
                .ForMember(
                    dst => dst.CategoryId,
                    opt => opt.MapFrom(src => src.Category)
                );

            CreateMap<ProductPropertyAdd, CategoryProperty>();
            CreateMap<ProductPropertyEdit, CategoryProperty>();

            CreateMap<CategoryProperty, ProductPropertyEdit>()
                .ForMember(
                    dst => dst.Category,
                    opt => opt.MapFrom(src => src.CategoryId)
                );

            CreateMap<CategoryProperty, ProductPropertyLookup>().ForMember(
                dst => dst.Category,
                opt => opt.MapFrom(src => src.Category.Name)
            );
            CreateMap<GetAllResult<CategoryProperty>, GetAllResultDto<ProductPropertyLookup>>();
        }
    }
}