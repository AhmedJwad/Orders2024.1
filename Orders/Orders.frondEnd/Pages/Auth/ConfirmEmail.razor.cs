using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class ConfirmEmail
    {
        private string? message;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

        protected async Task ConfirmAccountAsync()
        {
            var responseHttp = await repository.GetAsync($"/api/accounts/ConfirmEmail/?userId={UserId}&token={Token}");
            if (responseHttp.Error)
            {
                message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                navigationManager.NavigateTo("/");
                return;
            }

            await sweetAlertService.FireAsync("Confirmation", "Thank you for confirming your email, now you can log in to the system.", SweetAlertIcon.Info);
            navigationManager.NavigateTo("/Login");

        }

    }
}
