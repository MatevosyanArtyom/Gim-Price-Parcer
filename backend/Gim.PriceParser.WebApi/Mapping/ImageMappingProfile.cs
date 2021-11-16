using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.WebApi.Models.Image;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<ImageAddDto, GimImage>();
            CreateMap<GimImage, ImageLookupDto>()
                .IncludeAllDerived();

            CreateMap<GimImage, ImageFullDto>();
        }
    }
}