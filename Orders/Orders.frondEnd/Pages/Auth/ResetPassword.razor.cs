using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class ResetPassword
    {
        private ResetPasswordDTO ResetPasswordDTO = new();
        private bool loading;
        [Inject]private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private async Task ChangePasswordAsync()
        {
            ResetPasswordDTO.Token = Token;
            loading = true;
            var responseHttp = await repository.PostAsync("/api/accounts/ResetPassword", ResetPasswordDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                loading = false;
                return;
            }

            await sweetAlertService.FireAsync("Confirmation", "Password successfully changed, you can now log in with your new password.", SweetAlertIcon.Info);
            Modal.Show<Login>();


        }



    }
}
