using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountriesIndex
    {

        private int currentPage  = 1;
        private int totalPages;
        [Inject] private IRepository Reposotory { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;


        public List<Country>? countries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }
        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }
        private async Task LoadAsync(int page=1)
        {
            if(!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
           if(ok)
            {
             await LoadPagesAsync();

            }
        }

        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/Countries/totalPages?recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"?filter={Filter}";
            }
            var responseHttp = await Reposotory.GetASync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;


        }

        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/countries?page={page}&recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }
            var responseHttp = await Reposotory.GetASync<List<Country>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
            countries = responseHttp.Response;
            return true;
        }
        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 10;
            }
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
        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }
        private async Task FilterCallBack(string filter)
        {
            Filter = filter;
            await ApplyFilterAsync();
            StateHasChanged();
        }
        private async Task SelectedRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

    }
}
