using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Orders.frondEnd.Helpers;
using Orders.frondEnd.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Orders.frondEnd.AuthenticationProviders
{
    public class AuthenticationProviderJWT:AuthenticationProviderTest, ILoginService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly HttpClient _httpClient;
        private readonly string _tokenKey;
        private readonly AuthenticationState _anonimous;

        public AuthenticationProviderJWT(IJSRuntime jSRuntime, HttpClient httpClient)
        {
           _jSRuntime = jSRuntime;
           _httpClient = httpClient;
            _tokenKey = "TOKEN_KEY";
            _anonimous = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));

        }
        public async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jSRuntime.GetLocalStorage(_tokenKey);
            if(token is null)
            {
                return _anonimous;
            }
            return BuildAuthenticationState(token.ToString());
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            var claims = ParseClaimsFromJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private IEnumerable<Claim>? ParseClaimsFromJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var unserializedToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            return unserializedToken.Claims;

        }

        public async Task LoginAsync(string token)
        {
            await _jSRuntime.SetLocalStorage(_tokenKey, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        
        public async Task LogoutAsync()
        {
            await _jSRuntime.RemoveLocalStorage(_tokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));

        }
    }
}
