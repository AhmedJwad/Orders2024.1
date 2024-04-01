using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.States
{
    public partial class StateDetails
    {
        private State? state;

        [Parameter] public int StateId { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsyn();

        }

        private async Task LoadAsyn()
        {
            var responseHttp = await repository.GetASync<State>($"/api/States/Id?Id={StateId}");
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                    return;
                }
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
            state = responseHttp.Response;
        }

        private async Task DeleteAsync(City city)
        {
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title="Confirmation",
                Text= $"Do you really want to delete the city? {city.Name}",
                Icon= SweetAlertIcon.Question,
               ShowCancelButton= true,
               ConfirmButtonText="Yes",
               CancelButtonText="No",
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if(confirm)
            {
                return;
            }
            var responseHttp = await repository.DeleteAsync<City>($"/api/Cities/id?id={city.Id}");
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error );
                    return;
                }

            }
            await LoadAsyn();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowCancelButton = true,
                Timer = 3000,

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Record deleted successfully.");

        }
    }
}
