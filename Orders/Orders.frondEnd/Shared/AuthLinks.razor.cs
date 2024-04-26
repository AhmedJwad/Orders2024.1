﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Orders.frondEnd.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;

        [CascadingParameter]

        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            var claim= authenticationState.User.Claims.ToList();
            var photoClaim=claim.FirstOrDefault(X=>X.Type== "Photo");
            if(photoClaim is not null)
            {
                photoUser = photoClaim.Value;
            }

        }

    }
}