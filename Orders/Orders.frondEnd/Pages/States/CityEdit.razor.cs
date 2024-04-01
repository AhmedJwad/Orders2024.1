using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.States
{
    public partial class CityEdit
    {
        private City? city;
        private FormWithName<City>? cityForm;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Parameter] public int CityId { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await repository.GetASync<City>($"/api/Cities/id?id={CityId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Return();
                }
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            city = responseHttp.Response;
        }
        private async Task SaveAsync()

        {
            var responseHttp = await repository.PutAsync($"/api/cities", city);
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    var messgae = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", messgae, SweetAlertIcon.Question);
                    return;
                }

            }
            Return();
            var toast =  sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast=true,
                Position=SweetAlertPosition.BottomEnd,
                ShowConfirmButton=true,
                Timer=3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Changes saved successfully.");
        }

        private void Return()
        {
            cityForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo($"/states/details/{city!.StateId}");
        }
    }
}
