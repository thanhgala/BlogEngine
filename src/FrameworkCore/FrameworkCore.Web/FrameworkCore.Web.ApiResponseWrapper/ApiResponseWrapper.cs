using Microsoft.AspNetCore.Builder;
namespace FrameworkCore.Web.ApiResponseWrapper
{
    public static class ApiResponseWrapperExtension
    {
        public static IApplicationBuilder UseApiResponseAndExceptionWrapper(this IApplicationBuilder builder, ApiResponseWrapperOptions options = default)
        {
            options ??= new ApiResponseWrapperOptions();
            return builder.UseMiddleware<ApiResponseWrapperMiddleware>(options);
        }

        public static IApplicationBuilder UseApiResponseAndExceptionWrapper<T>(this IApplicationBuilder builder, ApiResponseWrapperOptions options = default)
        {
            options ??= new ApiResponseWrapperOptions();
            return builder.UseMiddleware<ApiResponseWrapperMiddleware<T>>(options);
        }
    }
}
