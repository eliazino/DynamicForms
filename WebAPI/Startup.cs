using AutoMapper;
using Core.Application.DTOs.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI {
    public class Startup {
        public static IWebHostEnvironment envr;
        public static SystemVariables appSettings;
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            Console.WriteLine(env.EnvironmentName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            string fileName = string.Concat("appsettings.", env.EnvironmentName, ".json");
            string fileName2 = string.Concat("appsettings.overrides.json");
            builder.AddJsonFile(fileName, optional: true);
            builder.AddJsonFile(fileName2, optional: true);
            Configuration = builder.Build();
            envr = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var appSettingsSection = Configuration.GetSection("SystemVariables");
            services.Configure<SystemVariables>(appSettingsSection);
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });            
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpContextAccessor();
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(Options => {
                Options.EnableForHttps = true;
                Options.Providers.Add<GzipCompressionProvider>();
            });
            var t = appSettingsSection.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = t.Hosts[0].Host + ":" + t.Hosts[0].Port;
            });
            var cnfg = new MapperConfiguration(cfg => {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            });
            var mapper = cnfg.CreateMapper();
            services.AddSingleton(mapper);
            Infrastructure.DI.Resolve.resolve(services);
            Core.Application.DI.Resolve.resolve(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              );
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseResponseCompression();
        }
    }
}
