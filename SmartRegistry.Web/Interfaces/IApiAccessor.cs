using System.Net.Http;

namespace SmartRegistry.Web.Interfaces
{
    public interface IApiAccessor
    {
        HttpClient GetHttpClient();
    }
}
