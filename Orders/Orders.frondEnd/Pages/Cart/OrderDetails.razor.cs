using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System.Net;

namespace Orders.frondEnd.Pages.Cart
{
    [Authorize(Roles ="Admin")]
    public partial class OrderDetails
    {
        private Order? order;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public int OrderId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHppt = await Repository.GetASync<Order>($"api/orders/{OrderId}");
            if (responseHppt.Error)
            {
                if (responseHppt.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/orders");
                    return;
                }
                var messageError = await responseHppt.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            order = responseHppt.Response;
        }

        private async void CancelOrderAsync()
        {
            await ModifyTemporalOrder("cancel", OrderStatus.Cancelled);

        }

        private async void DispatchOrderAsync()
        {
            await ModifyTemporalOrder("Dispatched", OrderStatus.Dispatched);

        }

        private async void SendOrderAsync()
        {
            await ModifyTemporalOrder("Sent", OrderStatus.Sent);

        }

        private async void ConfirmOrderAsync()
        {
            await ModifyTemporalOrder("Confirmed", OrderStatus.Confirmed);

        }
        private async Task ModifyTemporalOrder(string message , OrderStatus status)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {

                Title = "Confirmation",
                Text = $"Are you sure you want {message} the order?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true

            });
            var confirm = string.IsNullOrEmpty(message);
            if(confirm)
            {
                return;
            }
            var orderDTO = new OrderDTO
            {
                Id = OrderId,
                OrderStatus = status
            };
            var responseHttp = await Repository.PutAsync("api/orders", orderDTO);
            if (responseHttp.Error)
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }

            NavigationManager.NavigateTo("/orders");

        }
    }
}
