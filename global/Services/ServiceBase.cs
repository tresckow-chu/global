using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace global.Services
{
    public class ServiceBase : IServiceBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        private readonly ILocalStorageService _localStorage;

        public ServiceBase(HttpClient httpClient, ILocalStorageService localStorage)
        {
            //retur _config.GetSection("DatabaseSettings:ConnectionString").Value; //  GetValue<string>("DefaultConnection");
            //string Mike = ConfigurationHelper.config.GetSection("AWS:Accesskey").Value;

            _BaseUrl = ConfigurationHelper.config.GetSection("BaseUrl").Value;
            _httpClient = httpClient;

            _localStorage = localStorage;
        }

        public async Task<string> GetToken()
        {
            string tok = await _localStorage.GetItemAsync<string>(Constants.AppSettings.LocalStorageTokenName);

            if (string.IsNullOrWhiteSpace(tok))
                return null;

            return tok;         
        }

        public async Task DeleteLocalToken()
        {
           await _localStorage.RemoveItemAsync(Constants.AppSettings.LocalStorageTokenName);
        }

        public async Task<bool> IsToken()
        {
            var tok = await GetToken();
            return tok != null;
        }

        private static bool ValidateJsonContent(HttpContent content)
        {
            var mediaType = content?.Headers.ContentType?.MediaType;
            return mediaType != null && mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase);
        }

        protected async Task DeleteAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().DeleteAsync(FullUri);
            CheckResponse(response);
        }

        public async Task GetAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().GetAsync(FullUri);
            CheckResponse(response);
        }

        protected async Task<byte[]> GetByteArrayAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            try
            {
                return await GetHttpClient().GetByteArrayAsync(FullUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return default;
        }

        public async Task<T> GetJsonAsync<T>(string uri)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().GetAsync(FullUri, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);

            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }

            return default;
        }

        protected async Task<string> GetStringAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            try
            {
                return await GetHttpClient().GetStringAsync(FullUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return default;
        }

        public async Task PostAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().PostAsync(FullUri, null);
            CheckResponse(response);
        }

        public async Task<T> PostJsonAsync<T>(string uri, T value)
        {
            return await PostJsonAsync<T, T>(uri, value);
        }

        public async Task<TResult> PostJsonAsync<TValue, TResult>(string uri, TValue value)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().PostAsJsonAsync(FullUri, value);
            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                return result;
            }

            return default;
        }

        protected async Task PutAsync(string uri)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().PutAsync(FullUri, null);
            CheckResponse(response);
        }

        protected async Task<T> PutJsonAsync<T>(string uri, T value)
        {
            return await PutJsonAsync<T, T>(uri, value);
        }

        protected async Task<TResult> PutJsonAsync<TValue, TResult>(string uri, TValue value)
        {
            string FullUri = FullUrl(uri);

            var response = await GetHttpClient().PutAsJsonAsync(FullUri, value);
            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                return result;
            }
            return default;
        }

        private bool CheckResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;
            if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Request: {response.RequestMessage.RequestUri}");
                Console.WriteLine($"Response status: {response.StatusCode} {response.ReasonPhrase}");
            }

            return false;
        }

        public void RemoveAuthTokenFromHttpHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void AddAuthTokenToHttpHeader(string token)
        {
            const string TokenAuth = "Bearer";

            if (!_httpClient.DefaultRequestHeaders.Contains(TokenAuth))
            {
                //_httpClient.DefaultRequestHeaders.Add("Bearer", token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenAuth, token);

            }
        }

        private HttpClient GetHttpClient()
        {
            //if (!_httpClient.DefaultRequestHeaders.Contains(Constants.AntiForgeryTokenHeaderName) && _siteState != null && !string.IsNullOrEmpty(_siteState.AntiForgeryToken))
            //{
            //    _httpClient.DefaultRequestHeaders.Add(Constants.AntiForgeryTokenHeaderName, _siteState.AntiForgeryToken);
            //}
            return _httpClient;
        }

        private string FullUrl(string partialUrl)
        {
            return $"{_BaseUrl}/{partialUrl}";
        }
    }
}








//try
//{
//	//var requestMsg = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");

//	//var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"team/get-all-teams");

//	var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"https://tws.ppines.com/team-members/team/get-all-teams");


//	requestMsg.Headers.Add("Authorization", "Bearer " + await GetJWT());
//	var response = await Http.SendAsync(requestMsg);
//	if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
//	{
//		await jsr.InvokeVoidAsync("localStorage.removeItem", "app1-user").ConfigureAwait(false);
//		userdata = null;
//	}
//	else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
//		forecasts = null;
//	else if (response.IsSuccessStatusCode)
//		forecasts = await response.Content.ReadFromJsonAsync<Response>();
//}
//catch (Exception ex)
//{

//}
