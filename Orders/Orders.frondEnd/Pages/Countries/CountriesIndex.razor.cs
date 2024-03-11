using Microsoft.AspNetCore.Components;
using Orders.frondEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.frondEnd.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] private IRepository Reposotory { get; set; } = null;
        public List<Country>? countries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var responsehttp = await Reposotory.GetASync<List<Country>>("api/Countries");
            countries = responsehttp.Response;
        }
    }
}
