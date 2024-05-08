using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;
using System.Runtime.CompilerServices;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class RecoverPassword
    {
        private EmailDTO emailDTO = new();
        private bool loading;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        private async Task SendRecoverPasswordEmailTokenAsync()
        {
            loading = true;
            var responseHttp = await repository.PostAsync("/api/accounts/RecoverPassword", emailDTO);
            if(responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                loading = false;
                return;

            }
            loading = false;
            await sweetAlertService.FireAsync("Confirmation", "You have been sent an email with instructions to recover your password",
                SweetAlertIcon.Info);
            navigationManager.NavigateTo("/");

        }

    }
}
