using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadEye_Service.Data;
using RoadEye_Service.Repositories;
using RoadEye_Service.Services.ConstractPaginationHeader;
using RoadEye_Service.Services.GoogleApiService;
using RoadEye_Service.Services.RoadServices;
using System;

namespace RoadEye_Service
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public IHostingEnvironment Env { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            this.Env = env;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddHttpClient();

            string connectionString;
            if (Env.IsDevelopment()) {
                connectionString = Startup.Configuration["connectionString"];
            } else {
                connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            }

            services.AddDbContext<DataContext>(o => o.UseNpgsql(connectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddAutoMapper();
            services.AddCors();
            services.AddTransient<Seed>();
            services.AddScoped<IAnomalyRepository, AnomalyRepository>();
            services.AddScoped<IRoadRepository, RoadRepository>();
            services.AddScoped<IAnomalyTypeRepository, AnomalyTypeRepository>();
            services.AddScoped<IRoadConditionTypeRepository, RoadConditionTypeRepository>();
            services.AddScoped<IGoogleAPIRequestsManager, GoogleAPIRequestsManager>();
            services.AddScoped<IGoogleApiService, GoogleApiService>();
            services.AddScoped<IConstractPaginationHeaderService, ConstractPaginationHeaderService>();
            services.AddScoped<IUpdateRoadConditionService, UpdateRoadConditionService>();
            services.AddScoped<IAnomalyExistenceService, AnomalyExistenceService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory => {
                return new UrlHelper(implementationFactory.GetService<IActionContextAccessor>().ActionContext);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            seeder.SeedRoads();
            seeder.SeedAnomalies();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
