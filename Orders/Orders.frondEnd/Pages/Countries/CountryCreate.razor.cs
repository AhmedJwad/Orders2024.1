using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Countries
{
    [Authorize(Roles = "Admin")]

    public partial class CountryCreate
    {
        private Country country = new();

        private FormWithName<Country>? countryForm;

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
            countryForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/countries");
        }
    }
}
