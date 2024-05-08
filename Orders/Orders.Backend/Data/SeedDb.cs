

using Microsoft.EntityFrameworkCore;
using Orders.Backend.Services;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using Orders.Shared.Responses;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public SeedDb(DataContext context, IApiService apiService, IUsersUnitOfWork usersUnitOfWork)
        {
           _context = context;
           _apiService = apiService;
            _usersUnitOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync()
        {
           await _context.Database.EnsureCreatedAsync();
            //await CheckCountriesFullAsync();
            await CheckCountriesAsync();
           await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync( "Ahmed", "Almershady", "Ahmednet380@gmail.com", "+9647804468010", "Iraq/Babylon", UserType.Admin);


        }

        private async Task CheckCountriesFullAsync()
        {
            if (!_context.countries.Any())
            {
                var countriesStatesCitiesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesStatesCitiesSQLScript);
            }

        }

        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, string Adress, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if(user == null)
            {
                var city = await _context.cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                city ??= await _context.cities.FirstOrDefaultAsync();

                user = new()
                {
                    FirstName=firstName,
                    LastName=lastName,
                    Email=email,
                    PhoneNumber=phone,
                    UserName=email,
                    Address=Adress,
                    UserType=userType,
                    City=city,
                };
              await  _usersUnitOfWork.AddUserAsync(user, "123456");
              await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
              var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
              await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.countries.Any())
            {
                //var responseCountries = await _apiService.GetAsync<List<CountryResponse>>("/v1","/countries");
                //if(responseCountries.wasSuccess)
                //{
                //    var countries = responseCountries.Result!;
                //    foreach (var countryResponse in countries) 
                //    {
                //        var country = await _context.countries.FirstOrDefaultAsync(x => x.Name == countryResponse.Name!)!;
                //        if(country ==null)
                //        {
                //            country=new Country()
                //            {
                //                Name=countryResponse.Name!, states=new List<State>(),

                //            };
                //            var responseState = await _apiService.GetAsync<List<StateResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states");
                //            if(responseState.wasSuccess)
                //            {
                //                var states = responseState.Result!;
                //                foreach (var stateResponse in states)
                //                {
                //                    var state = country.states!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                //                    if(state ==null)
                //                    {
                //                        state=new State() { Name=stateResponse.Name!,cities=new List<City>() };
                //                    }
                //                    var responseCities = await _apiService.GetAsync<List<CityResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                //                    if (responseCities.wasSuccess)
                //                    {
                //                        var cities = responseCities.Result!;
                //                        foreach (var CityResponse in cities)
                //                        {
                //                            if (CityResponse.Name == "Mosfellsbær" || CityResponse.Name == "Șăulița")
                //                            {
                //                                continue;
                //                            }
                //                            var city = state.cities!.FirstOrDefault(c => c.Name == CityResponse.Name!)!;
                //                            if (city == null)
                //                            {
                //                                state.cities!.Add(new City() { Name = CityResponse.Name! });
                //                            }

                //                        }
                //                        if (state.CityNumber > 0)
                //                        {
                //                            country.states.Add(state);
                //                        }
                //                    }
                //                }
                //            }
                //            if (country.StatesNumber > 0)
                //            {
                //                _context.countries.Add(country);
                //                await _context.SaveChangesAsync();
                //            }
                //        }

                //    }
                //}
                 _context.countries.Add(new Country
                {
                    Name = "Iraq",
                    states =
                   [
                       new State()
                        {
                            Name = "Baghdad",
                            cities = [
                                new() { Name = "Almonsour" },
                                new() { Name = "Alkarda" },
                                new() { Name = "he Abonk" },
                                new() { Name = "Aldora" },
                                new() { Name = "Aljama" },
                                new() { Name = "Alaadhama" },
                            ]
                        },
                        new State()
                        {
                            Name = "babil",
                           cities = [
                                new() { Name = "Alkaram" },
                                new() { Name = "hey Alhussein" },
                                new() { Name = "Aljameaa" },
                                new() { Name = "40 street" },
                                new() { Name = "60 street" },
                            ]
                        },
                    ]
                });
                _context.countries.Add(new Country
                {
                    Name = "Oman",
                    states =
                    [
                        new State()
                        {
                            Name = "Florida",
                            cities = [
                                new() { Name = "masqat1" },
                                new() { Name = "masqat2" },
                                new() { Name = "masqat3" },
                                new() { Name = "masqat14" },
                                new() { Name = "masqat15" },
                            ]
                        },
                        new State()
                        {
                            Name = "Texas",
                            cities = [
                                new() { Name = "Houston" },
                                new() { Name = "San Antonio" },
                                new() { Name = "Dallas" },
                                new() { Name = "Austin" },
                                new() { Name = "El Paso" },
                            ]
                        },
                    ]
                });
            }

        

    }
        private async Task CheckCategoriesAsync()
        {
            if(!_context.categories.Any())
            {
                _context.categories.Add(new Category { Name = "computers" });
                _context.categories.Add(new Category { Name = "Accessories" });
            }
            await _context.SaveChangesAsync();
        }
    }
}
