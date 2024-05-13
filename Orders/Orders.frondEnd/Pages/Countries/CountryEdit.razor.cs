using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;
using System.Diagnostics.Metrics;

namespace Orders.frondEnd.Pages.Countries
{
    [Authorize(Roles = "Admin")]

    public partial class CountryEdit
    {
        private Country? Country { get; set; }
        private FormWithName<Country>? countryForm;

        [Inject] private IRepository Repository { get; set; } = null;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null;
        [Inject] private NavigationManager navigationManager { get; set; } = null;
        [EditorRequired , Parameter] public int Id { get; set; }
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetASync<Country>($"/api/Countries/{Id}");
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message=await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
            }
            else
            {
                Country = responseHttp.Response;
            }

        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/Countries", Country);
            if(responseHttp.Error) 
            { 
                var message= await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await BlazoredModal.CloseAsync(ModalResult.Ok());
            Return();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Changes saved successfully.\"");
        }

        private void Return()
        {
            countryForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/countries");
        }
    }
}
