using System.Linq;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Product;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductAdd, Product>()
                .ForMember(dst => dst.Category, opt => opt.Ignore())
                .ForMember(
                    dst => dst.CategoryId,
                    opt => opt.MapFrom(src => src.Category)
                )
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                );

            CreateMap<ProductEdit, Product>()
                .ForMember(dst => dst.Category, opt => opt.Ignore())
                .ForMember(
                    dst => dst.CategoryId,
                    opt => opt.MapFrom(src => src.Category)
                )
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                );

            CreateMap<Product, ProductBase>()
                .IncludeAllDerived()
                .ForMember(
                    dst => dst.Category,
                    opt => opt.MapFrom(src => src.CategoryId)
                )
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.SupplierId)
                );

            CreateMap<Product, ProductEdit>();

            CreateMap<Product, ProductLookup>()
                .BeforeMap((src, dst) =>
                {
                    var categories = src.Category.Ancestors.ToList();
                    categories.Add(src.CategoryId);
                    dst.Category1 = categories.FirstOrDefault();
                    dst.Category2 = categories.Skip(1).FirstOrDefault();
                    dst.Category3 = categories.Skip(2).FirstOrDefault();
                    dst.Category4 = categories.Skip(3).FirstOrDefault();
                    dst.Category5 = categories.Skip(4).FirstOrDefault();
                });

            CreateMap<GetAllResult<Product>, GetAllResultDto<ProductLookup>>();
        }
    }
}