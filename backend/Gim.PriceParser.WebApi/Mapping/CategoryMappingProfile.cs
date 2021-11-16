using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Category;
using Gim.PriceParser.WebApi.Util;

namespace Gim.PriceParser.WebApi.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryAdd, Category>()
                .ForMember(dst => dst.Parent, opt => opt.Ignore())
                .ForMember(
                    dst => dst.ParentId,
                    opt => opt.MapFrom(src => src.Parent)
                );
            CreateMap<CategoryEdit, Category>()
                .ForMember(dst => dst.Parent, opt => opt.Ignore())
                .ForMember(
                    dst => dst.ParentId,
                    opt => opt.MapFrom(src => src.Parent)
                );

            CreateMap<Category, CategoryEdit>()
                .ForMember(
                    dst => dst.Parent,
                    opt => opt.MapFrom(src => src.ParentId)
                );

            CreateMap<Category, CategoryLookup>()
                .ForMember(
                    dst => dst.Parent,
                    opt => opt.MapFrom(src => src.ParentId)
                )
                .ForMember(
                    dst => dst.RootParent,
                    opt => opt.MapFrom(src =>
                        src.Path.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()));

            CreateMap<TreeItem<Category>, TreeItem<CategoryLookup>>();

            CreateMap<Category, TreeItem<CategoryLookup>>()
                .ConvertUsing((src, dst, ctx) =>
                {
                    var item = ctx.Mapper.Map<CategoryLookup>(src);
                    return new TreeItem<CategoryLookup>
                        {Item = item};
                });

            CreateMap<CategoriesTreeResult, List<TreeItem<CategoryLookup>>>()
                .ConvertUsing((src, dst, context) =>
                    context.Mapper.Map<List<TreeItem<CategoryLookup>>>(
                        src.Children.GenerateTree(x => x.Id, x => x.ParentId, src.Matched?.ParentId)));

            CreateMap<GetAllResult<Category>, GetAllResultDto<CategoryLookup>>();
        }
    }
}