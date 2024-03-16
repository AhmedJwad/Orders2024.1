using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Categories
{
    public partial class CategoryForm
    {
        public EditContext editContext = null!;

        [EditorRequired, Parameter]  public Category category { get; set; } = null!;
        [EditorRequired , Parameter] public EventCallback onValidSubmit { get; set; }
        [EditorRequired , Parameter] public EventCallback ReturnAction { get; set; }
        [Inject] public SweetAlertService sweetAlertService { get; set; }
        public bool FormPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(category);

        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formwadedited = editContext.IsModified();
            if(!formwadedited || FormPostedSuccessfully)
            {
                return;
            }
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmation",
                Text = "Do you want to leave the page and lose the changes?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}
