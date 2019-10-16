using Newtonsoft.Json;

namespace FrameworkCore.Web.ApiResponseWrapper.Contracts
{
    internal interface IJsonSettings
    {
        JsonSerializerSettings GetJSONSettings(bool ignoreNull, bool useCamelCaseNaming = true);
    }
}
