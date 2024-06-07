using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Cart
{
    [Authorize(Roles ="Admin , User")]
    public partial class OrdersIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private int currentPage = 1;
        private int totalPages;

        public List<Order>? Orders { get; set; }
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;
        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;

        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task SelectedRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 10;
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/orders?page={page}&recordsnumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&Filter={Filter}";
            }
            var response = await repository.GetASync<List<Order>>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Orders = response.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/orders/totalPages?recordsnumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }
            var response = await repository.GetASync<int>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = response.Response;
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
    }
}
