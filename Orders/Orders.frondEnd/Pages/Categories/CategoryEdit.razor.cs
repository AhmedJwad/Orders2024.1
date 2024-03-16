using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.frondEnd.Pages.Categories
{
    public partial class CategoryEdit
    {
        private Category? category { get; set; }
        private CategoryForm? categoryForm { get; set; }
        [Inject] private IRepository repository { get; set; } = null;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null;
        [Inject] private NavigationManager navigationManager { get; set; } = null;
        [EditorRequired, Parameter]public int id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await repository.GetASync<Category>($"/api/Ctagories/id?id={id}");
            if(responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/categories");
                }
                else
                {
                    var messsage = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", messsage, SweetAlertIcon.Error);
                }
            }
            else
            {
                category = responseHttp.Response;
            }
        }
        private async Task EditAsync()
        {
            var resposeHttp = await repository.PutAsync($"/api/Ctagories", category);
            if(resposeHttp.Error)
            {
                var message = await resposeHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
           await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Changes saved successfully.");
        }

        private void Return()
        {
            categoryForm.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/Categories");
        }
    }
}
