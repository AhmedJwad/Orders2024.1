

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
                                new City { Name = "Al-Adel" },
                                new City { Name = "Al-Amal" },
                                new City { Name = "Al-Amin" },
                                new City { Name = "Al-Baiyaa" },
                                new City { Name = "Al-Baladiyat" },
                                new City { Name = "Al-Bayaa" },
                                new City { Name = "Al-Binook" },
                                new City { Name = "Al-Dora" },
                                new City { Name = "Al-Furat" },
                                new City { Name = "Al-Furat Al-Awsat" },
                                new City { Name = "Al-Ghazaliyah" },
                                new City { Name = "Al-Habibiyah" },
                                new City { Name = "Al-Hurriya" },
                                new City { Name = "Al-Ilam" },
                                new City { Name = "Al-Jadida" },
                                new City { Name = "Al-Kadhimiya" },
                                new City { Name = "Al-Karrada" },
                                new City { Name = "Al-Khudra" },
                                new City { Name = "Al-Kifah" },
                                new City { Name = "Al-Mada'in" },
                                new City { Name = "Al-Mansour" },
                                new City { Name = "Al-Mashtal" },
                                new City { Name = "Al-Ma'moun" },
                                new City { Name = "Al-Muthanna" },
                                new City { Name = "Al-Nahda" },
                                new City { Name = "Al-Nidhal" },
                                new City { Name = "Al-Rahmaniya" },
                                new City { Name = "Al-Rasheed" },
                                new City { Name = "Al-Risala" },
                                new City { Name = "Al-Saadoun" },
                                new City { Name = "Al-Sadr City" },
                                new City { Name = "Al-Salhiyah" },
                                new City { Name = "Al-Saydiyah" },
                                new City { Name = "Al-Shaab" },
                                new City { Name = "Al-Sho'ala" },
                                new City { Name = "Al-Sulaikh" },
                                new City { Name = "Al-Thawra" },
                                new City { Name = "Al-Waziriya" },
                                new City { Name = "Bab Al-Muadham" },
                                new City { Name = "Bab Al-Sharqi" },
                                new City { Name = "Hayy Al-Andalus" },
                                new City { Name = "Hayy Al-Amel" },
                                new City { Name = "Hayy Al-Amin" },
                                new City { Name = "Hayy Al-Bakr" },
                                new City { Name = "Hayy Al-Dhahir" },
                                new City { Name = "Hayy Al-Furat" },
                                new City { Name = "Hayy Al-Ghadeer" },
                                new City { Name = "Hayy Al-Hartha" },
                                new City { Name = "Hayy Al-Hurriya" },
                                new City { Name = "Hayy Al-Jihad" },
                                new City { Name = "Hayy Al-Jihad Al-Islami" },
                                new City { Name = "Hayy Al-Karakh" },
                                new City { Name = "Hayy Al-Karkh" },
                                new City { Name = "Hayy Al-Maalif" },
                                new City { Name = "Hayy Al-Mahdi" },
                                new City { Name = "Hayy Al-Mashtal" },
                                new City { Name = "Hayy Al-Mua'alimin" },
                                new City { Name = "Hayy Al-Muallimin" },
                                new City { Name = "Hayy Al-Muhandisin" },
                                new City { Name = "Hayy Al-Mujahidin" },
                                new City { Name = "Hayy Al-Murur" },
                                new City { Name = "Hayy Al-Nahdha" },
                                new City { Name = "Hayy Al-Qadiriya" },
                                new City { Name = "Hayy Al-Qadisiya" },
                                new City { Name = "Hayy Al-Qanat" },
                                new City { Name = "Hayy Al-Rashid" },
                                new City { Name = "Hayy Al-Sadeer" },
                                new City { Name = "Hayy Al-Sarai" },
                                new City { Name = "Hayy Al-Saydiyah" },
                                new City { Name = "Hayy Al-Shoula" },
                                new City { Name = "Hayy Al-Suwaib" },
                                new City { Name = "Hayy Al-Taji" },
                                new City { Name = "Hayy Al-Tayaran" },
                                new City { Name = "Hayy Al-Yarmouk" },
                                new City { Name = "Hayy Al-Zayuna" },
                                new City { Name = "Hayy Al-Zuhur" },
                                new City { Name = "Hayy Amin" },
                                new City { Name = "Hayy Arab Jabour" },
                                new City { Name = "Hayy As-Salam" },
                                new City { Name = "Hayy Attar" },
                                new City { Name = "Hayy Babil" },
                                new City { Name = "Hayy Fadl" },
                                new City { Name = "Hayy Jameela" },
                                new City { Name = "Hayy Karrada" },
                                new City { Name = "Hayy Malikiya" },
                                new City { Name = "Hayy Salman" },
                                new City { Name = "Hayy Shorja" },
                                new City { Name = "Hayy Ur" },
                                new City { Name = "Kadhimiya" },
                                new City { Name = "Madinat Al-Sadr" },
                                new City { Name = "Masbah" },
                                new City { Name = "New Baghdad" },
                                new City { Name = "Old Baghdad" },
                                new City { Name = "Sadr City" },
                                new City { Name = "Tayaran" }
                            ]
                        },
                       new State()
                        {
                            Name = "Al Anbar",
                           cities = new List<City>
                            {
                                new City { Name = "Ramadi" },
                                new City { Name = "Fallujah" },
                                new City { Name = "Haditha" },
                                new City { Name = "Hit" },
                                new City { Name = "Rawa" },
                                // Add more cities as needed
                            }
                        },
                        new State()
                        {
                            Name = "Al Basrah",
                            cities = new List<City>
                            {
                                new City { Name = "Basra" },
                                new City { Name = "Umm Qasr" },
                                new City { Name = "Al-Zubair" },
                                new City { Name = "Shatt al-Arab" },
                                new City { Name = "Al-Faw" },
                                // Add more cities as needed
                            }
                        },
                        new State()
                        {
                            Name = "Al Muthanna",
                            cities = new List<City>
                            {
                                new City { Name = "Samawah" },
                                new City { Name = "Al-Rumaytha" },
                                new City { Name = "Al-Khidhir" },
                                new City { Name = "Al-Qasim" },
                                new City { Name = "Al-Diwaniyah" },
                                // Add more cities as needed
                            }
                        },
                        // Add more states and their cities here
                        new State()
                        {
                            Name = "Babil",
                            cities = new List<City>
                            {
                                new City { Name = "Alkaram" },
                                new City { Name = "hey Alhussein" },
                                new City { Name = "Aljameaa" },
                                new City { Name = "40 street" },
                                new City { Name = "60 street" },
                                // Add more cities as needed
                            }
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
