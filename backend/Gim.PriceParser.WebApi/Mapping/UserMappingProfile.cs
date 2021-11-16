using System;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Account;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserAdd, GimUser>()
                .ForMember(
                    dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Today))
                .ForMember(dst => dst.Password,
                    opt => opt.MapFrom(src => ""));

            CreateMap<UserEdit, GimUser>();
            CreateMap<GimUser, UserEdit>();

            CreateMap<GimUser, UserLookup>()
                .ForMember(
                    dst => dst.AccessRights,
                    opt => opt.MapFrom(src => src.Role.AccessRights));

            CreateMap<GimUser, string>().ConvertUsing(src => src.Fullname);
            CreateMap<GetAllResult<GimUser>, GetAllResultDto<UserLookup>>();
        }
    }
}