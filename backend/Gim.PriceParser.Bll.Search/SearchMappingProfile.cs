using System.Linq;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Search.Models;

namespace Gim.PriceParser.Bll.Search
{
    public class SearchMappingProfile : Profile
    {
        public SearchMappingProfile()
        {
            CreateMap<Product, ProductEs>()
                .ForMember(
                    dst => dst.InnerId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dst => dst.Properties,
                    opt => opt.MapFrom(src =>
                        src.Properties.Where(prop => !string.IsNullOrWhiteSpace(prop.Name)).Select(prop => prop.Name)));
        }
    }
}