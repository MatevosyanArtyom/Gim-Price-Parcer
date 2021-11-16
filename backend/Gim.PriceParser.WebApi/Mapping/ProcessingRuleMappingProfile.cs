using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.ProcessingRule;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class ProcessingRuleMappingProfile : Profile
    {
        public ProcessingRuleMappingProfile()
        {
            CreateMap<ProcessingRuleAdd, ProcessingRule>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier));

            CreateMap<ProcessingRuleFull, ProcessingRule>()
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(
                    dst => dst.SupplierId,
                    opt => opt.MapFrom(src => src.Supplier));

            CreateMap<ProcessingRule, ProcessingRuleFull>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.SupplierId));

            CreateMap<ProcessingRule, ProcessingRuleLookup>()
                .ForMember(
                    dst => dst.Supplier,
                    opt => opt.MapFrom(src => src.Supplier.Name));

            CreateMap<GetAllResult<ProcessingRule>, GetAllResultDto<ProcessingRuleLookup>>();
        }
    }
}