using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.SchedulerTask;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class SchedulerTaskMappingProfile : Profile
    {
        public SchedulerTaskMappingProfile()
        {
            CreateMap<EmitResult, EmitResultDto>();

            CreateMap<Diagnostic, DiagnosticDto>()
                .ForMember(dst => dst.Message, opt => opt.MapFrom(src => src.ToString()));

            CreateMap<SchedulerTaskAdd, SchedulerTask>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                );

            CreateMap<SchedulerTaskEdit, SchedulerTask>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier)
                );

            CreateMap<SchedulerTask, SchedulerTaskEdit>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.SupplierId)
                );

            CreateMap<SchedulerTask, SchedulerTaskFull>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.SupplierId)
                );

            CreateMap<SchedulerTask, SchedulerTaskLookup>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.Supplier.Name)
                );

            CreateMap<GetAllResult<SchedulerTask>, GetAllResultDto<SchedulerTaskLookup>>();
        }
    }
}