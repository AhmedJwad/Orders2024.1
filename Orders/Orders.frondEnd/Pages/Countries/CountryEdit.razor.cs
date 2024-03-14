using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;
using System.Diagnostics.Metrics;

namespace Orders.frondEnd.Pages.Countries
{
    public partial class CountryEdit
    {
        private Country? Country { get; set; }
        private CountryForm? countryForm { get; set; }

        [Inject] private IRepository Repository { get; set; } = null;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null;
        [Inject] private NavigationManager navigationManager { get; set; } = null;
        [EditorRequired , Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetASync<Country>($"/api/Countries/id?id={Id}");
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
            countryForm!.formPostedSuccessfully = true;
            navigationManager.NavigateTo("/countries");
        }
    }
}
