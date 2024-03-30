using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] private IRepository Reposotory { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        public List<Country>? countries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Reposotory.GetASync<List<Country>>("/api/Countries");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
            countries = responseHttp.Response;
        }
        private async Task DeleteAsync(Country country)
        {
            var result = await SweetAlertService.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmation",
                    Text = $"Are you sure you want to delete the country:{country.Name}?",
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,

                });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm) { return; }
            var responseHttp = await Reposotory.DeleteAsync<Country>($"/api/Countries/id?id={country.Id}");
            if(responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/countries");
                }
                else
                {
                    var messerror = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messerror, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast=true,
                Position=SweetAlertPosition.BottomEnd,
                ShowConfirmButton=true,
                Timer=3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Log successfully deleted");
        }
    }
}
