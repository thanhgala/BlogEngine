﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrameworkCore.Web.ApiResponseWrapper.Extensions;
using FrameworkCore.Web.ApiResponseWrapper.Extensions.Wrappers;
using FrameworkCore.Web.ApiResponseWrapper.Helpers;

namespace FrameworkCore.Web.ApiResponseWrapper
{
    internal class ApiResponseWrapperMembers
    {
        private readonly ApiResponseWrapperOptions _options;
        private readonly ILogger<ApiResponseWrapperMiddleware> _logger;
        private readonly JsonSerializerSettings _jsonSettings;
        public readonly Dictionary<string, string> _propertyMappings;
        private readonly bool _isCustomObjectUsed;

        public ApiResponseWrapperMembers(ApiResponseWrapperOptions options,
            ILogger<ApiResponseWrapperMiddleware> logger, JsonSerializerSettings jsonSettings,
            Dictionary<string, string> propertyMappings = null, bool isCustomObjectUsed = false)
        {
            _options = options;
            _logger = logger;
            _jsonSettings = jsonSettings;
            _propertyMappings = propertyMappings;
            _isCustomObjectUsed = isCustomObjectUsed;
        }

        public async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Method} {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        public async Task<string> FormatResponse(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        public Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            object apiError;
            int code;

            if (exception is ApiException ex)
            {
                if (ex.IsModelValidatonError)
                {
                    apiError = new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ex.Errors)
                    {
                        ReferenceErrorCode = ex.ReferenceErrorCode,
                        ReferenceDocumentLink = ex.ReferenceDocumentLink,
                    };

                    _logger.Log(LogLevel.Warning, ex, $"[{ex.StatusCode}]: {ResponseMessage.ValidationError}");
                }
                else if (ex.IsCustomErrorObject) //new addition
                {
                    apiError = ex.CustomError;
                    _logger.Log(LogLevel.Warning, ex, $"[{ex.StatusCode}]: {ResponseMessage.Exception}");
                }
                else
                {
                    apiError = new ApiError(ex.Message)
                    {
                        ReferenceErrorCode = ex.ReferenceErrorCode,
                        ReferenceDocumentLink = ex.ReferenceDocumentLink,
                    };

                    _logger.Log(LogLevel.Warning, ex, $"[{ex.StatusCode}]: {ResponseMessage.Exception}");
                }

                code = ex.StatusCode;

            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
                code = (int)HttpStatusCode.Unauthorized;

                _logger.Log(LogLevel.Warning, exception, $"[{code}]: {ResponseMessage.UnAuthorized}");
            }
            else
            {

                string exceptionMessage = string.Empty;
                string stackTrace = null;

                if (_options.IsDebug)
                {
                    exceptionMessage = $"{ exceptionMessage } { exception.GetBaseException().Message }";
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    exceptionMessage = ResponseMessage.Unhandled;
                }

                apiError = new ApiError(exceptionMessage) { Details = stackTrace };
                code = (int)HttpStatusCode.InternalServerError;

                _logger.Log(LogLevel.Error, exception, $"[{code}]: {exceptionMessage}");
            }

            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));

            return WriteFormattedResponseToHttpContext(context, code, jsonString, true);
        }

        public Task HandleNotSuccessRequestAsync(HttpContext context, int code)
        {
            ApiError apiError = WrapError(code);
            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));
            return WriteFormattedResponseToHttpContext(context, code, jsonString, true);
        }

        public Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            string jsonString;

            var bodyText = !body.ToString().IsValidJson() ? ConvertToJSONString(body) : body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);

            Type type = bodyContent?.GetType();

            if (type == typeof(JObject))
            {
                if (_propertyMappings == null || _propertyMappings.Count == 0)
                {

                    var jsonSettings = JSONHelper.GetJSONSettings(_options.IgnoreNullValue, _options.UseCamelCaseNamingStrategy);
                    return WriteFormattedResponseToHttpContext(context, code, JsonConvert.SerializeObject(bodyContent, jsonSettings));
                }

                ApiResponse apiResponse = new ApiResponse();
                if (_isCustomObjectUsed && _propertyMappings.Count > 0)
                {
                    var obj = _propertyMappings;
                    JToken jtStatusCode = _options.UseCamelCaseNamingStrategy ? bodyContent?[obj[Prop.StatusCode].ToCamelCase()] : bodyContent[obj[Prop.StatusCode]];
                    JToken jtResult = _options.UseCamelCaseNamingStrategy ? bodyContent?[obj[Prop.Result].ToCamelCase()] : bodyContent[obj[Prop.Result]];
                    if (!jtStatusCode.IsNullOrEmpty() && !jtResult.IsNullOrEmpty())
                    {
                        var statusCode = (int)jtStatusCode;
                        apiResponse.StatusCode = statusCode == 0 ? code : statusCode;
                        apiResponse.Result = jtResult;
                    }
                    else
                    {
                        throw new ApiException(ResponseMessage.NoMappingFound);
                    }
                }
                else
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                }

                //ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                if (apiResponse.StatusCode == 0 && apiResponse.Result == null && apiResponse.ResponseException == null)
                    jsonString = ConvertToJSONString(code, bodyContent);
                else if ((apiResponse.StatusCode != code || apiResponse.Result != null) ||
                        (apiResponse.StatusCode == code && apiResponse.Result == null))
                {
                    code = apiResponse.StatusCode; // in case response is not 200 (e.g 201, etc)
                    jsonString = ConvertToJSONString(GetSucessResponse(apiResponse));

                }
                else
                    jsonString = ConvertToJSONString(code, bodyContent);
            }
            else
            {
                jsonString = ConvertToJSONString(code, bodyContent);
            }

            return WriteFormattedResponseToHttpContext(context, code, jsonString);
        }

        public Task HandleSpaSupportAsync(HttpContext context)
        {
            string configErrorText = ResponseMessage.NotApiOnly;
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(configErrorText);
            return context.Response.WriteAsync(configErrorText);
        }

        public bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/swagger"));
        }

        public bool IsApi(HttpContext context)
        {
            if (_options.IsApiOnly && !context.Request.Path.Value.Contains(".js") && !context.Request.Path.Value.Contains(".css"))
                return true;

            return context.Request.Path.StartsWithSegments(new PathString(_options.WrapWhenApiPathStartsWith));
        }

        #region Private Members

        private Task WriteFormattedResponseToHttpContext(HttpContext context, int code, string jsonString, bool isError = false)
        {
            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = jsonString != null ? Encoding.UTF8.GetByteCount(jsonString) : 0;
            return context.Response.WriteAsync(jsonString);
        }


        private string ConvertToJSONString(int code, object content)
        {
            code = !_options.ShowStatusCode ? 0 : code;
            return JsonConvert.SerializeObject(new ApiResponse(ResponseMessage.Success, content, code, GetApiVersion()), _jsonSettings);
        }

        private string ConvertToJSONString(ApiResponse apiResponse)
        {
            apiResponse.StatusCode = !_options.ShowStatusCode ? 0 : apiResponse.StatusCode;
            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToJSONString(ApiError apiError)
        {
            return JsonConvert.SerializeObject(apiError, _jsonSettings);
        }

        private string ConvertToJSONString(object rawJSON)
        {
            return JsonConvert.SerializeObject(rawJSON, _jsonSettings);
        }

        private ApiError WrapError(int statusCode)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    return new ApiError(ResponseMessage.NotFound);
                case (int)HttpStatusCode.NoContent:
                    return new ApiError(ResponseMessage.NotContent);
                case (int)HttpStatusCode.MethodNotAllowed:
                    return new ApiError(ResponseMessage.MethodNotAllowed);
                default:
                    return new ApiError(ResponseMessage.Unknown);
            }
        }

        private ApiResponse GetErrorResponse(int code, object apiError)
        {
            code = !_options.ShowStatusCode ? 0 : code;
            return new ApiResponse(code, apiError) { Version = GetApiVersion() };
        }

        private ApiResponse GetSucessResponse(ApiResponse apiResponse)
        {
            apiResponse.Message ??= ResponseMessage.Success;
            apiResponse.Version = GetApiVersion();
            return apiResponse;
        }

        private string GetApiVersion()
        {
            return !_options.ShowApiVersion ? null : _options.ApiVersion;
        }

        #endregion
    }
}
