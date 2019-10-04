using FrameworkCore.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Autofac;
using Blog.Infrastructure.CrossCutting.IoC;
using Microsoft.Extensions.Hosting;
using FrameworkCore.Web.AzureIdentity;
using Microsoft.Identity.Web;

namespace Blog.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddProtectWebApiWithMicrosoftIdentityPlatformV2(Configuration);

            services.AddProtectedWebApi(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMediatR(typeof(Startup));

            //services.AddMediatR(typeof(GetAllBlogsQueryHandler).GetTypeInfo().Assembly);

            services.AddAutoMapperCore();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //configure auto fac here
            builder.RegisterModule(new InfrastructureLayerInjector());
            builder.RegisterModule(new DomainLayerInjector());
            builder.RegisterModule(new ApplicationLayerInjector());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
