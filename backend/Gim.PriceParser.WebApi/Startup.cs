using System;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Bll.Mail;
using Gim.PriceParser.Bll.Scheduler;
using Gim.PriceParser.Bll.Search;
using Gim.PriceParser.Bll.Services;
using Gim.PriceParser.Dal.Common;
using Gim.PriceParser.Dal.Impl.Mongo;
using Gim.PriceParser.Dal.Impl.Mongo.DbSettings;
using Gim.PriceParser.Processor;
using Gim.PriceParser.WebApi.Mapping;
using Gim.PriceParser.WebApi.Util;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Gim.PriceParser.WebApi
{
    public class Startup
    {
        private const string MongoConnectionStringEniromentVariableName = "GimPriceParser_MongoConnection";
        private const string ElasticConnectionStringEniromentVariableName = "GimPriceParser_ElasticConnection";

        public static IConfiguration Configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .Build();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
            //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddOptions();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GIM price parser API",
                    Version = "v1"
                });

                opt.OperationFilter<MakeOperationIdFilter>();

                //opt.SchemaFilter<EnumAsSeparateTypeFilter>();

                // это надо для того, чтобы для дженериковых типов генерировались валидные идентификаторы
                opt.CustomSchemaIds(type => type.FriendlyId().Replace("[", "Of").Replace(",", "And").Replace("]", ""));
            });

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDb"));
            services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));
            services.Configure<MailSettings>(settings =>
            {
                var hostName = Configuration.GetValue<string>("MAIL_HOSTNAME");
                settings.HostName = hostName;
            });

            services.Configure<ElasticSearchSettings>(Configuration.GetSection("ElasticSearch"));
            services.Configure<ElasticSearchSettings>(settings =>
            {
                var envConnStr = Configuration.GetConnectionString(ElasticConnectionStringEniromentVariableName);
                if (!string.IsNullOrWhiteSpace(envConnStr))
                {
                    settings.ConnectionString = envConnStr;
                }
            });


            services.AddSingleton<IMongoDbSettings>(sp =>
            {
                var mongoSettings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var envConnStr = Configuration.GetConnectionString(MongoConnectionStringEniromentVariableName);
                if (!string.IsNullOrWhiteSpace(envConnStr))
                {
                    mongoSettings.ConnectionString = envConnStr;
                }

                return mongoSettings;
            });

            services.AddSingleton(AutoMapperConfigurator.GetMapper());

            services.AddTransient<IPasswordHasher<GimUser>, PasswordHasher<GimUser>>();

            services.AddHttpContextAccessor();

            DalModule.ConfigureServices(services);
            ServicesModule.ConfigureServices(services);
            SearchModule.ConfigureServices(services);
            MailModule.ConfigureServices(services);
            SchedulerModule.ConfigureServices(services);
            ProcessorModule.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(opt =>
            {
                opt.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(opt => { opt.SwaggerEndpoint("/swagger/v1/swagger.json", "GIM price parser API v1"); });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHangfireDashboard();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var configurer = serviceScope.ServiceProvider.GetService<IDbConfigurer>() ?? throw new ArgumentNullException(nameof(IDbConfigurer));
                configurer.ConfigureIndexes();

                var seeder = serviceScope.ServiceProvider.GetService<IDbSeeder>() ?? throw new ArgumentNullException(nameof(IDbSeeder));
                seeder.Seed();

                var search = serviceScope.ServiceProvider.GetService<IElasticConfigurer>() ?? throw new ArgumentNullException(nameof(IElasticConfigurer));
                search.Configure();
            }

            //RecurringJob.AddOrUpdate<IMailClient>(x => x.ReceiveMessagesAsync(), Cron.Minutely());
            RecurringJob.AddOrUpdate<IProcessorClient>(x => x.ParsePriceListsAsync(), Cron.Minutely(),
                queue: HangfireQueues.Parser);
            RecurringJob.AddOrUpdate<IProcessorClient>(x => x.DownloadImages(), Cron.Minutely(),
                queue: HangfireQueues.ImageDownloader);

            //handle client side routes
            app.Run(async context =>
            {
                var indexFileLocation = Path.Combine(env.WebRootPath, "index.html");
                if (File.Exists(indexFileLocation))
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(indexFileLocation);
                }
                else
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                }
            });
        }
    }
}
