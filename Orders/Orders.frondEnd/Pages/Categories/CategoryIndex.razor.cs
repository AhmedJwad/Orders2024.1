using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;
using System.Diagnostics.Metrics;

namespace Orders.frondEnd.Pages.Categories
{
    public partial class CategoryIndex
    {
        [Inject] private IRepository repository { get; set; } = null;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null;
        [Inject] private NavigationManager navigationManager { get; set; } = null;

        public List<Category> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();

        }

        private async Task LoadAsync()
        {
            var reposteryHttp = await repository.GetASync<List<Category>>("api/Ctagories");
            if(reposteryHttp.Error)
            {
                var message = await reposteryHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
            Categories = reposteryHttp.Response!;
        }
        private async Task DeleteAsync(Category category)
        {
            var result = await sweetAlertService.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmation",
                    Text = $"Are you sure you want to delete the country:{category.Name}?",
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,

                });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm) { return; }
            var responseHttp = await repository.DeleteAsync<Category>($"/api/Ctagories/id?id={category.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/Categories");
                }
                else
                {
                    var messerror = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", messerror, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadAsync();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Log successfully deleted");
        }
    }
}
