using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Orders.Shared.Entities;
using Microsoft.AspNetCore.Components.Routing;
namespace Orders.frondEnd.Pages.Countries
{
    public partial class CountryForm
    {
        private EditContext editContext = null;

        [EditorRequired, Parameter] public Country Country { get; set; } = null!;
        [EditorRequired, Parameter] public EventCallback onValidSubmit { get; set; }
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
        [Inject] public SweetAlertService sweetAlertService { get; set; } = null!;
        public bool formPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(Country);

        }
        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var FormwadEditor = editContext.IsModified();
            if (!FormwadEditor || formPostedSuccessfully)
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
