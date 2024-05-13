using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.frondEnd.Pages.Categories
{
    [Authorize(Roles = "Admin")]

    public partial class CategoryEdit
    {
        private Category? category { get; set; }
        private FormWithName<Category>? categoryForm;
        [Inject] private IRepository repository { get; set; } = null;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null;
        [Inject] private NavigationManager navigationManager { get; set; } = null;
        [EditorRequired, Parameter]public int id { get; set; }
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await repository.GetASync<Category>($"/api/Categories/id?id={id}");
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
            var resposeHttp = await repository.PutAsync($"/api/Categories", category);
            if(resposeHttp.Error)
            {
                var message = await resposeHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
           
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await BlazoredModal.CloseAsync(ModalResult.Ok());
            Return();
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Changes saved successfully.");
        }
        

        private void Return()
        {
            categoryForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/categories");
        }
    }
}
