using Microsoft.AspNetCore.Components.Authorization;
using Orders.Shared.Entities;
using System.Security.Claims;

namespace Orders.frondEnd.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           
            var anonimous = new ClaimsIdentity();
            var user = new ClaimsIdentity(authenticationType: "test");
            var admin = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Ahmed"),
                new Claim("LastName", "Almershady"),
                new Claim(ClaimTypes.Name, "Ahmednet380@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }
        
    }
}
