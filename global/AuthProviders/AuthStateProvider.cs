using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BPSGobalClient.Services;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BPSGobalClient.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        //private readonly AuthenticationState _anonymous;

        private const string AuthType = "jwtAuthType";        

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            //_anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState Anonm = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var token = await _localStorage.GetItemAsync<string>(Constants.AppSettings.LocalStorageTokenName);
            if (string.IsNullOrWhiteSpace(token))
                return Anonm;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var AllClaimsExtracted = JwtParser.ParseClaimsFromJwt(token);

            ClaimsIdentity identity = new ClaimsIdentity(AllClaimsExtracted, AuthType);

            return new AuthenticationState(new ClaimsPrincipal(identity));


            ////Works with micrsoft auth

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, "John Doe"),
            //    new Claim(ClaimTypes.Role, "Administrator")
            //};

            //var anonymous = new ClaimsIdentity(claims, "testAuthType");
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));

        }

        public void NotifyUserAuthentication(string token)
        {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthType));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            //NotifyAuthenticationStateChanged(authState);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            //Need to destroy the token in local storage!!!!!

            //var authState = Task.FromResult(_anonymous);
            //NotifyAuthenticationStateChanged(authState);

            _localStorage.RemoveItemAsync(Constants.AppSettings.LocalStorageTokenName);

            ClaimsIdentity identity = new ClaimsIdentity();
            var AS = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(AS);
        }
    }
}
