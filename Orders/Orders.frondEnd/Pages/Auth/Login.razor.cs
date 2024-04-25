using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Services;
using Orders.Shared.DTOs;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class Login
    {
        private LoginDTO loginDTO = new();

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private ILoginService loginService { get; set; } = null!;

        private async Task LoginAsync()
        {
            var responseHttp = await repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await loginService.LoginAsync(responseHttp.Response!.Token);
            navigationManager.NavigateTo("/");


        }
    }
}
