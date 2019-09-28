using Blog.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Data
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<BlogDbContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
