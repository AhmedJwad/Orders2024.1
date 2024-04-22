using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.States
{
    [Authorize(Roles = "Admin")]

    public partial class CityCreate
    {
        private City city = new ();

        private FormWithName<City>? cityForm;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] IRepository repository { get; set; } = null!;
        [Parameter] public int StateId { get; set; }
        
        private async Task CreateAsync()
        {
            city.StateId = StateId;
            var responseHttp = await repository.PostAsync("/api/cities", city);
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
               
            }
            Return();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowCancelButton = true,
                Timer = 3000,
            });
            await sweetAlertService.FireAsync(icon: SweetAlertIcon.Success, message: "Record created successfully.");
        }

        private void Return()
        {
            cityForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo($"/states/details/{StateId}");
        }
    }
}
