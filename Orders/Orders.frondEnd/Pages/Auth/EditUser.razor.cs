using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.frondEnd.Services;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.frondEnd.Pages.Auth
{
    public partial class EditUser
    {
        private User? user;
        private List<Country>? countries ;
        private List<State>? states ;
        private List<City>? cities ;
        private string? imageUrl;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; }=null!;
        [Inject] private ILoginService loginService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsyc();
            await LoadCountriesAsync();
            await LoadStatesAsyn(user!.City!.State!.Country!.Id);
            await LoadCitiesAsyn(user!.City!.State!.Id);

            if(!string.IsNullOrEmpty(user!.Photo))
            {
                //imageUrl = user.Photo;
                var imageName = Path.GetFileName(user.Photo);
                imageUrl = $"{navigationManager.BaseUri}images/users/{imageName}";
                user.Photo = null;
            }

        }

       

        private async Task LoadUserAsyc()
        {
            var responseHttp = await repository.GetASync<User>($"/api/accounts");
            if(responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/");
                    return;
                }
                var messageError = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            user = responseHttp.Response;
        }

        private void ImageSelected(string imagenBase64)
        {
            user!.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var selectedCountry = Convert.ToInt32(e.Value);
            states = null;
            cities = null;
            user!.CityId = 0;
            await LoadStatesAsyn(selectedCountry);

        }

      
        private async Task StateChangedAsync(ChangeEventArgs e)
        {
           var selectedState=Convert.ToInt32(e.Value);
            cities = null;
            user!.CityId = 0;
            await LoadCitiesAsyn(selectedState);
        }

       

        private async Task LoadCountriesAsync()
        {
            var responseHttp = await repository.GetASync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            countries = responseHttp.Response;
        }
        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await repository.GetASync<List<State>>($"/api/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            states = responseHttp.Response;

        }
        private async Task LoadCitiesAsyn(int stateId)
        {
            var responseHttp = await repository.GetASync<List<City>>($"/api/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;

        }
        private async Task SaveUserAsync()
        {
            var responseHttp = await repository.PutAsync<User>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            navigationManager.NavigateTo("/");
        }

    }
}
