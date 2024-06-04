using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Cart
{
    [Authorize(Roles ="Admin, User")]
    public partial class ShowCart
    {
        public List<TemporalOrder>? temporalOrders { get; set; }
        public float sumQuantity;
        public decimal sumValue;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get;  set; } = null!;

        public OrderDTO orderDTO = new();

        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();

        }

        private async Task LoadAsync()
        {
            try
            {
                var responseHttp = await Repository.GetASync<List<TemporalOrder>>("api/temporalOrders/my");
                temporalOrders = responseHttp.Response!;
                sumQuantity = temporalOrders.Sum(x => x.Quantity);
                sumValue=temporalOrders.Sum(x => x.Value);
            }
            catch (Exception Ex)
            {

                await SweetAlertService.FireAsync("Error", Ex.Message, SweetAlertIcon.Error);

            }
        }
        private async Task Delete(int temporalOrderId)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmation",
                Text = "Are you sure you want to delete the record??",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            var confirm = string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<TemporalOrder>($"api/TemporalOrders/id?id={temporalOrderId}");

            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/");
                    return;
                }

                var mensajeError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
                return;
            }

            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = false,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Product removed from shopping cart.");

        }
        private async void ConfirmOrderAsync()
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title= "Confirmation",
                Text= "Are you sure you want to confirm the order?",
                Icon=SweetAlertIcon.Question,
                ShowCancelButton=true,
                
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var httpActionResponse = await Repository.PostAsync("/api/orders", orderDTO);
            if (httpActionResponse.Error)
            {
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            navigationManager.NavigateTo("/Cart/OrderConfirmed");


        }

    }
}
