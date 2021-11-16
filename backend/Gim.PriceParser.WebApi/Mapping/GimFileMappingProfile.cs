using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.WebApi.Models.GimFile;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class GimFileMappingProfile : Profile
    {
        public GimFileMappingProfile()
        {
            CreateMap<GimFileAdd, GimFile>();
            CreateMap<GimFile, GimFileFull>();
            CreateMap<GimFile, GimFileLookup>();
        }
    }
}