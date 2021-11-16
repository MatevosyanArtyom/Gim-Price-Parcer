using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.SupplierProduct;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class SupplierProductMappingProfile : Profile
    {
        public SupplierProductMappingProfile()
        {
            CreateMap<SupplierProductAdd, SupplierProduct>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                )
                .ForMember(dst => dst.Product, opt => opt.Ignore())
                .ForMember(
                    dst => dst.ProductId,
                    opt => opt.MapFrom(src => src.Product)
                );

            CreateMap<SupplierProductEdit, SupplierProduct>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                )
                .ForMember(dst => dst.Product, opt => opt.Ignore())
                .ForMember(
                    dst => dst.ProductId,
                    opt => opt.MapFrom(src => src.Product)
                );

            CreateMap<SupplierProduct, SupplierProductEdit>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.SupplierId)
                ).ForMember(
                    dst => dst.Product,
                    opt => opt.MapFrom(src => src.ProductId)
                );

            CreateMap<SupplierProduct, SupplierProductLookup>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.Supplier.Name)
                )
                .ForMember(
                    dst => dst.Product,
                    opt => opt.MapFrom(src => src.Product.Id)
                );

            CreateMap<GetAllResult<SupplierProduct>, GetAllResultDto<SupplierProductLookup>>();
        }
    }
}