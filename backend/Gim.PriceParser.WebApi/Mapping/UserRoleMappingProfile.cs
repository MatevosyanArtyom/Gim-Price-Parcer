using System;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.UserRole;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class UserRoleMappingProfile : Profile
    {
        public UserRoleMappingProfile()
        {
            CreateMap<AccessRightsDto, AccessRights>().ReverseMap();

            CreateMap<UserRoleAdd, GimUserRole>()
                .ForMember(
                    dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Today));
            CreateMap<UserRoleEdit, GimUserRole>();
            CreateMap<GimUserRole, UserRoleEdit>();
            CreateMap<GimUserRole, UserRoleLookup>();
            CreateMap<GetAllResult<GimUserRole>, GetAllResultDto<UserRoleLookup>>();
        }
    }
}