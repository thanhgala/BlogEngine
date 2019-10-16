using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FrameworkCore.Web.ApiResponseWrapper.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrameworkCore.Web.ApiResponseWrapper
{
    internal class ApiResponseWrapperMiddleware : WrapperBase
    {
        private readonly ApiResponseWrapperMembers _awm;
        public ApiResponseWrapperMiddleware(RequestDelegate next, ApiResponseWrapperOptions options, ILogger<ApiResponseWrapperMiddleware> logger) : base(next, options, logger)
        {
            var jsonSettings = Helpers.JSONHelper.GetJSONSettings(options.IgnoreNullValue, options.UseCamelCaseNamingStrategy);
            _awm = new ApiResponseWrapperMembers(options, logger, jsonSettings);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await InvokeAsyncBase(context, _awm);
        }
    }

    internal class ApiResponseWrapperMiddleware<T> : WrapperBase
    {
        private readonly ApiResponseWrapperMembers _awm;
        public ApiResponseWrapperMiddleware(RequestDelegate next, ApiResponseWrapperOptions options, ILogger<ApiResponseWrapperMiddleware> logger) : base(next, options, logger)
        {
            var tup = Helpers.JSONHelper.GetJSONSettings<T>(options.IgnoreNullValue, options.UseCamelCaseNamingStrategy);
            _awm = new ApiResponseWrapperMembers(options, logger, tup.Settings, tup.Mappings, true);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await InvokeAsyncBase(context, _awm);
        }
    }
}
