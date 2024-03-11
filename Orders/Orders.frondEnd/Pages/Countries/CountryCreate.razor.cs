using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Countries
{
    public partial class CountryCreate
    {
        private Country country = new();

        private CountryForm? countryForm;

        [Inject]private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            var reposteryHttp = await repository.PostAsync("/api/countries", country);
            if(reposteryHttp.Error)
            {
                var message=await reposteryHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return ();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowCancelButton = true,
                Timer=3000,
            }) ;
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Record created successfully.");
        }

        private void Return()
        {
            countryForm!.formPostedSuccessfully = true;
            navigationManager.NavigateTo("/countries");
        }
    }
}
