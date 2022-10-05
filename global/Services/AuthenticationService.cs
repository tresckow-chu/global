using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using global.AuthProviders;
using global.Entities;

namespace global.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        //private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage) : base(client, localStorage)
        {
            //_client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        //public async Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration)
        //{
        //    var content = JsonSerializer.Serialize(userForRegistration);
        //    var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        //    var registrationResult = await _client.PostAsync("accounts/registration", bodyContent);
        //    var registrationContent = await registrationResult.Content.ReadAsStringAsync();

        //    if (!registrationResult.IsSuccessStatusCode)
        //    {
        //        var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);
        //        return result;
        //    }

        //    return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        //}


        public async Task<LoginResult> Login(LoginModel login)
        {
            var loginResult = await PostJsonAsync<LoginModel, LoginResult>("admin/login", login);

            if(loginResult != null)
            {
                string Tok = loginResult.Token.Value;
                await _localStorage.SetItemAsync(Constants.AppSettings.LocalStorageTokenName, Tok);
                AddAuthTokenToHttpHeader(Tok);

                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(Tok);
            }

            return new LoginResult { IsSuccess = true };
        }


        //public async Task<LoginResult> Login(LoginModel login)
        //{
        //    var content = JsonSerializer.Serialize(login);
        //    var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        //    var authResult = await _client.PostAsync("https://tws.ppines.com/team-members/admin/login", bodyContent);
        //    var authContent = await authResult.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<LoginResult>(authContent, _options);

        //    if (!authResult.IsSuccessStatusCode)
        //        return result;

        //    await _localStorage.SetItemAsync(Constants.AppSettings.LocalStorageTokenName, result.Token.Value);
        //    ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token.Value);
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token.Value);

        //    return new LoginResult { IsSuccess = true };
        //}

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(Constants.AppSettings.LocalStorageTokenName);
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            RemoveAuthTokenFromHttpHeader();
        }
    }
}
