using Blazored.Modal;
using Blazored.Modal.Services;
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
        private bool wasClose;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        private async Task CloseModalAsync()
        {
            wasClose = true;
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }
        private async Task LoginAsync()
        {
            if (wasClose)
            {
                navigationManager.NavigateTo("/");
                return;
            }
            if(loginDTO.Email == null || loginDTO.Password==null)
            {
               
                return;
            }
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
