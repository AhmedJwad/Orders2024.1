using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.DTOs;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class ResendConfirmationEmailToken
    {
        private EmailDTO emailDTO = new();
        private bool Loading;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!; 

        private async Task ResendConfirmationEmailTokenAsync()
        {
            Loading = true;
            var responseHttp = await repository.PostAsync("/api/accounts/ResedToken", emailDTO);
            Loading= false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                Loading = false;
                return;
            }

            await sweetAlertService.FireAsync("Confirmation", "You have been sent an email with instructions to activate your username.", SweetAlertIcon.Info);
            navigationManager.NavigateTo("/");

        }
    }
}
