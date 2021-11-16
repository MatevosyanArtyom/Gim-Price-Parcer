using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Supplier;
using Microsoft.AspNetCore.Http;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile()
        {
            CreateMap<ContactPerson, ContactPersonDto>().ReverseMap();

            CreateMap<FiasEntity, string>().ConvertUsing(src => src.Value);

            CreateMap<SupplierAdd, Supplier>()
                //.ForMember(
                //    dst => dst.UserId,
                //    opt => opt.MapFrom<UserIdResolver>())
                .ForMember(
                    dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Today));

            CreateMap<SupplierEdit, Supplier>()
                .ForMember(dst => dst.User, opt => opt.Ignore())
                .ForMember(
                    dst => dst.UserId,
                    opt => opt.MapFrom(src => src.User));

            CreateMap<Supplier, SupplierEdit>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.UserId));

            CreateMap<Supplier, SupplierLookup>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.User.Fullname));
            
            CreateMap<Supplier, SupplierShort>();

            CreateMap<EntityVersion<Supplier>, EntityVersionDto<SupplierEdit>>()
                .ForMember(
                    dst => dst.User,
                    opt => opt.MapFrom(src => src.User.Fullname));
            CreateMap<GetAllResult<EntityVersion<Supplier>>, GetAllResultDto<EntityVersionDto<SupplierEdit>>>();

            CreateMap<GetAllResult<Supplier>, GetAllResultDto<SupplierLookup>>();
        }
    }

    public class UserIdResolver : IValueResolver<SupplierAdd, Supplier, string>
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserIdResolver(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string Resolve(SupplierAdd source, Supplier destination, string destMember, ResolutionContext context)
        {
            var sid = _httpContext.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            return sid?.Value;
        }
    }
}