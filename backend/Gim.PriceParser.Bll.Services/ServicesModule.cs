using Gim.PriceParser.Bll.Services.Categories;
using Gim.PriceParser.Bll.Services.PriceListItems;
using Gim.PriceParser.Bll.Services.PriceLists;
using Gim.PriceParser.Bll.Services.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Gim.PriceParser.Bll.Services
{
    public static class ServicesModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPriceListService, PriceListService>();
            services.AddTransient<IPriceListItemService, PriceListItemService>();
            services.AddTransient<IProductService, ProductService>();
        }
    }
}