using System.Net;
using System.Net.Http.Json;

namespace global.Services
{
    public interface IServiceBase
    {
        Task GetAsync(string uri);

        Task PostAsync(string uri);

        Task<T> PostJsonAsync<T>(string uri, T value);

        Task<TResult> PostJsonAsync<TValue, TResult>(string uri, TValue value);

        void RemoveAuthTokenFromHttpHeader();
        void AddAuthTokenToHttpHeader(string token);

        Task<string> GetToken();
        Task<bool> IsToken();
    }
}
