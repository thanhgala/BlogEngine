namespace FrameworkCore.Infrastructure.Services
{
    public interface IWebApiRestService
    {
        T For<T>(string webApiHost) where T : IWebApiServiceBase;
        T ForWithoutAuthorize<T>(string webApiHost) where T : IWebApiServiceBase;
    }
}
