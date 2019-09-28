using System;
using FrameworkCore.Identity.Web;
using FrameworkCore.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog.Infrastructure.CrossCutting.IoC;


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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            //services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            //{
            //    options.Authority += "/v2.0";

            //    // The valid audiences are both the Client ID (options.Audience) and api://{ClientID}
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidAudiences = new[] {options.Audience, $"api://{options.Audience}"}
            //    };

            //    // If you want to debug, or just understand the JwtBearer events, uncomment the following line of code
            //    // options.Events = JwtBearerMiddlewareDiagnostics.Subscribe(options.Events);
            //});

            services.AddProtectWebApiWithMicrosoftIdentityPlatformV2(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMediatR(typeof(Startup));

            //services.AddMediatR(typeof(GetAllBlogsQueryHandler).GetTypeInfo().Assembly);

            services.AddAutoMapperCore();

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new InfrastructureLayerInjector());
            container.RegisterModule(new DomainLayerInjector());
            container.RegisterModule(new ApplicationLayerInjector());

            return new AutofacServiceProvider(container.Build());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
        app.UseAuthentication();
        app.UseMvc();
    }
}
}
