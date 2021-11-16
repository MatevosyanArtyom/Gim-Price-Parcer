using System;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.PriceList;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class PriceListMappingProfile : Profile
    {
        public PriceListMappingProfile()
        {
            CreateMap<PriceListAdd, PriceList>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                )
                .ForMember(dst => dst.ProcessingRule, opt => opt.Ignore())
                .ForMember(
                    dst => dst.ProcessingRuleId,
                    opt => opt.MapFrom(src => src.ProcessingRule)
                )
                .ForMember(dst => dst.SchedulerTask, opt => opt.Ignore())
                .ForMember(
                    dst => dst.CreatedDate,
                    opt => opt.MapFrom(src => DateTime.Today))
                .ForMember(
                    dst => dst.SchedulerTaskId,
                    opt => opt.MapFrom(src => src.SchedulerTask)
                )
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => PriceListStatus.InQueue));

            CreateMap<PriceList, PriceListLookup>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.Supplier.Name)
                )
                .ForMember(
                    dst => dst.SchedulerTask,
                    opt => opt.MapFrom(src => src.SchedulerTask.Name)
                )
                .ForMember(
                    dst => dst.ProcessingRule,
                    opt => opt.MapFrom(src => src.ProcessingRule.Name)
                )
                .ForMember(
                    dst => dst.Author,
                    opt => opt.MapFrom(src => src.Author.Fullname));

            CreateMap<PriceList, PriceListFull>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.Supplier.Name)
                )
                .ForMember(
                    dst => dst.SchedulerTask,
                    opt => opt.MapFrom(src => src.SchedulerTask.Name)
                )
                .ForMember(
                    dst => dst.ProcessingRule,
                    opt => opt.MapFrom(src => src.ProcessingRule.Name)
                )
                .ForMember(
                    dst => dst.Author,
                    opt => opt.MapFrom(src => src.Author.Fullname));

            CreateMap<GetAllResult<PriceList>, GetAllResultDto<PriceListLookup>>();
        }
    }
}