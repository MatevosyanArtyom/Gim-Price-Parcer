using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Manufacturer;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ManufacturerMappingProfile : Profile
    {
        public ManufacturerMappingProfile()
        {
            CreateMap<ManufacturerAdd, Manufacturer>();
            CreateMap<ManufacturerEdit, Manufacturer>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerLookup>();

            CreateMap<GetAllResult<Manufacturer>, GetAllResultDto<ManufacturerLookup>>();
        }
    }
}