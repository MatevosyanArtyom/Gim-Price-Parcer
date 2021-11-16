using System.Linq;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.WebApi.Models.PriceListItem;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class PriceListItemMappingProfile : Profile
    {
        public PriceListItemMappingProfile()
        {
            CreateMap<ProductSynonym, ProductSynonymDto>();

            CreateMap<PriceListItemPropertyMatched, PriceListItemProductPropertyLookup>()
                .ForMember(
                    dst => dst.ProductValue,
                    opt => opt.MapFrom(src => src.ProductValue.Name))
                .ForMember(
                    dst => dst.Property,
                    opt => opt.MapFrom(src => src.Property.Name))
                .ForMember(
                    dst => dst.Value,
                    opt => opt.MapFrom(src => src.Value.Name));

            CreateMap<PriceListItemMatched, PriceListItemLookup>()
                .ForMember(
                    dst => dst.Product,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(
                    dst => dst.HasSynonyms,
                    opt => opt.MapFrom(src => src.ProductSynonyms.Any()));
        }
    }
}