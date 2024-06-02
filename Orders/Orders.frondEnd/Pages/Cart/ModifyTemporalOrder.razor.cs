using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Cart
{
    [Authorize(Roles = "Admin , User")]
    public partial class ModifyTemporalOrder
    {
        private List<string>? categories;
        private List<string>? images;
        private bool loading = true;
        private Product? product;
        private TemporalOrderDTO? TemporalOrderDTO;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] IRepository repository { get; set; } = null!;
        [Parameter] public int TemporalOrderId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTemporalOrderAsync();

        }

        private async Task LoadTemporalOrderAsync()
        {
            loading = true;
            var httpResponse = await repository.GetASync<TemporalOrder>($"/api/temporalOrders/{TemporalOrderId}");
            if (httpResponse.Error)
            {
                loading = false;
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;

            }
            var temporalOrder = httpResponse.Response;
            TemporalOrderDTO = new TemporalOrderDTO
            {
                Id = temporalOrder.Id,
                ProductId = temporalOrder.ProductId,
                Quantity = temporalOrder.Quantity,
                Remarks = temporalOrder!.Remarks!,
            };
            product = temporalOrder.Product;
            categories = product.ProductCategories!.Select(x => x.Category.Name).ToList();
            images = product.ProductImages?.Select(x => x.Image).ToList();
            loading = false;

        }

        private async Task UpdateCartAsync()
        {
            var httpResponse = await repository.PutAsync("/api/temporalOrders/full", TemporalOrderDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;

            }
            var toast2 = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast2.FireAsync(icon: SweetAlertIcon.Success, message: "Modified product in the purchases.");
            NavigationManager.NavigateTo("/");


        }
    }
    }
