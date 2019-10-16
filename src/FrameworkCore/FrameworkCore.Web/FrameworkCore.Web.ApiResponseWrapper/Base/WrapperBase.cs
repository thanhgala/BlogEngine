using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FrameworkCore.Web.ApiResponseWrapper.Extensions;

namespace FrameworkCore.Web.ApiResponseWrapper.Base
{
    internal abstract class WrapperBase
    {
        private readonly RequestDelegate _next;
        private readonly ApiResponseWrapperOptions _options;
        private readonly ILogger<ApiResponseWrapperMiddleware> _logger;

        protected WrapperBase(RequestDelegate next, ApiResponseWrapperOptions options, ILogger<ApiResponseWrapperMiddleware> logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public virtual async Task InvokeAsyncBase(HttpContext context, ApiResponseWrapperMembers awm)
        {
            if (awm.IsSwagger(context) || !awm.IsApi(context))
                await this._next(context);
            else
            {

                var stopWatch = Stopwatch.StartNew();

                var request = await awm.FormatRequest(context.Request);

                var originalBodyStream = context.Response.Body;

                await using var newBodyStream = new MemoryStream();
                try
                {
                    context.Response.Body = newBodyStream;
                    await _next.Invoke(context);
                    context.Response.Body = originalBodyStream;

                    var bodyAsText = await awm.FormatResponse(newBodyStream);

                    if (context.Response.StatusCode != 304)
                    {

                        if (!_options.IsApiOnly && bodyAsText.IsHtml() && context.Response.StatusCode == 200)
                            context.Response.StatusCode = 404;

                        if (!context.Request.Path.StartsWithSegments(new PathString(_options.WrapWhenApiPathStartsWith))
                            && bodyAsText.IsHtml() && context.Response.StatusCode == 200)
                        {
                            if (newBodyStream.Length > 0)
                            {
                                await awm.HandleSpaSupportAsync(context);
                            }
                        }
                        else if (context.Response.StatusCode == 200)
                        {
                            await awm.HandleSuccessRequestAsync(context, bodyAsText, context.Response.StatusCode);
                        }
                        else
                        {
                            await awm.HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                        }
                    }

                }
                catch (Exception ex)
                {
                    await awm.HandleExceptionAsync(context, ex);
                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    await newBodyStream.CopyToAsync(originalBodyStream);
                }
                finally
                {
                    stopWatch.Stop();
                    _logger.Log(LogLevel.Information, $@"Request: {request} Responded with [{context.Response.StatusCode}] in {stopWatch.ElapsedMilliseconds}ms");
                }
            }

        }
    }
}
