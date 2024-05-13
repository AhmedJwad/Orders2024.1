using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.DTOs;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class ChangePassword
    {
        private ChangePasswordDTO changePasswordDTO = new();
        private bool Loading;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private async Task ChangePasswordAsync()
        {
            Loading = true;
            var responseHttp = await repository.PostAsync("/api/accounts/changePassword", changePasswordDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                Loading = false;
                return;
            }

            Loading = false;
            await BlazoredModal.CloseAsync(ModalResult.Ok());
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Password changed successfully.");
        }

    }
}

