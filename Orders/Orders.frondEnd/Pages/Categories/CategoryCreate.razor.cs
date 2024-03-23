using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Categories
{
    public partial class CategoryCreate
    {
        private Category category = new();
        private FormWithName<Category> categoryForm { get; set; }
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            var repsotryHttp = await repository.PostAsync("/api/Ctagories", category);
            if(repsotryHttp.Error)
            {
                var message = await repsotryHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();

            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast=true,
                Position=SweetAlertPosition.BottomEnd,
                ShowConfirmButton=true,
                Timer=3000,
            });

            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Record created successfully.");
        }

        private void Return()
        {
            categoryForm!.FormPostedSuccessfully=true;
            navigationManager.NavigateTo("/categories");
        }
    }
}
