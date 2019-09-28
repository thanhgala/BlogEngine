using FrameworkCore.Infrastructure.Attribute;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.Services
{
    public class WebApiRestService : IWebApiRestService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebApiRestService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<string> GetToken() => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        public T For<T>(string webApiHost) where T : IWebApiServiceBase
        {
            var routeHost = GetRouteHost<T>(webApiHost);
            return RestService.For<T>(new HttpClient(
                new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new Uri(routeHost)
            });
        }

        public T ForWithoutAuthorize<T>(string webApiHost) where T : IWebApiServiceBase
        {
            var routeHost = GetRouteHost<T>(webApiHost);
            return RestService.For<T>(routeHost);
        }

        private static string GetRouteHost<T>(string webApiHost) where T : IWebApiServiceBase
        {
            if (string.IsNullOrEmpty(webApiHost))
            {

            }
            var prefix = typeof(T).GetCustomAttributes<WebApiRouteAttribute>(false).FirstOrDefault();
            var route = "";
            if (prefix != null)
                route = prefix.Route;
            return webApiHost + route;
        }
    }
}
