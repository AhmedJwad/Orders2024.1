

using Microsoft.EntityFrameworkCore;
using Orders.Backend.Helpers;
using Orders.Backend.Services;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using Orders.Shared.Responses;
using System;
using System.Runtime.InteropServices;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IFileStoragecs _fileStorage;

        public SeedDb(DataContext context, IApiService apiService, IUsersUnitOfWork usersUnitOfWork, IFileStoragecs
            fileStorage)
        {
           _context = context;
           _apiService = apiService;
            _usersUnitOfWork = usersUnitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
           await _context.Database.EnsureCreatedAsync();
            //await CheckCountriesFullAsync();
            await CheckCountriesAsync();
           await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync( "Ahmed", "Almershady", "Ahmednet380@gmail.com", "+9647804468010", "Iraq/Babylon", "noimage.jpg", UserType.Admin);
            await CheckUserAsync( "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Brad", "Pitt", "brad@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Angelina", "Jolie", "angelina@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Bob", "Marley", "bob@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Celia", "Cruz", "celia@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.Admin);
            await CheckUserAsync( "Fredy", "Mercury", "fredy@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Hector", "Lavoe", "hector@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Liv", "Taylor", "liv@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Otep", "Shamaya", "otep@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Ozzy", "Osbourne", "ozzy@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckUserAsync( "Selena", "Quintanilla", "selenba@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpg", UserType.User);
            await CheckProductsAsync();


        }

        private async Task CheckCountriesFullAsync()
        {
            if (!_context.countries.Any())
            {
                var countriesStatesCitiesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesStatesCitiesSQLScript);
            }

        }
        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
              
                await AddProductAsync("Sport Shoes - Adidas Barracuda", 270000M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Sport Shoes - Adidas Superstar", 250000M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Fresh Avocado", 5000M, 500F, new List<string>() { "Food" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Apple AirPods", 1300000M, 12F, new List<string>() { "Technology", "Apple" }, new List<string>() { "noimage.png", "noimage.png" });
                await AddProductAsync("Akai Music Production Controller - APC40 MKII", 2650000M, 12F, new List<string>() { "Technology" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Apple Watch Series Ultra", 4500000M, 24F, new List<string>() { "Apple", "Technology" }, new List<string>() { "noimage.png", "noimage.png" });
                await AddProductAsync("Bose Noise Cancelling Headphones", 870000M, 12F, new List<string>() { "Technology" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Ribble Racing Bicycle", 12000000M, 6F, new List<string>() { "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Classic Plaid Shirt", 56000M, 24F, new List<string>() { "Clothing" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Cycling Helmet", 820000M, 12F, new List<string>() { "Sports" }, new List<string>() { "noimage.png", "noimage.png" });
                await AddProductAsync("Sports Sunglasses", 160000M, 24F, new List<string>() { "Sports" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Triple Meat Burger Combo", 25500M, 240F, new List<string>() { "Food" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Apple iPad", 2300000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Apple iPhone 13", 5200000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Johnnie Walker Blue Label Whisky - 750ml", 1266700M, 18F, new List<string>() { "Spirits" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Inflatable Rooster Costume", 150000M, 28F, new List<string>() { "Toys" }, new List<string>() { "noimage.png", "noimage.png", "noimage.png" });
                await AddProductAsync("Apple Mac Book Pro", 12100000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Adjustable Dumbbells", 370000M, 12F, new List<string>() { "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Hydrating Face Mask", 26000M, 100F, new List<string>() { "Beauty" }, new List<string>() { "noimage.png" });
                await AddProductAsync("New Balance Running Shoes - 530", 180000M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("New Balance Running Shoes - 565", 179000M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Nike Air Running Shoes", 233000M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Nike Zoom Running Shoes", 249900M, 12F, new List<string>() { "Footwear", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Adidas Women's Sports Sweatshirt", 134000M, 12F, new List<string>() { "Clothing", "Sports" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Boost Original Nutritional Supplement", 15600M, 12F, new List<string>() { "Nutrition" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Whey Protein Supplement", 252000M, 12F, new List<string>() { "Nutrition" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Pet Harness", 25000M, 12F, new List<string>() { "Pets" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Pet Bed", 99000M, 12F, new List<string>() { "Pets" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Gaming Keyboard", 67000M, 12F, new List<string>() { "Gamer", "Technology" }, new List<string>() { "noimage.png" });
                await AddProductAsync("Luxury Car Wheel Ring - 17", 1600000M, 33F, new List<string>() { "Automobiles" }, new List<string>() { "noimage.png", "noimage.png" });
                await AddProductAsync("Gaming Chair", 980000M, 12F, new List<string>() { "Gamer", "Technology" }, new List<string>() { "noimage.png" });
                           
                await _context.SaveChangesAsync();

            }

        }
        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product product = new()
            {
                Name = name,
                Description = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>(),
            };
            foreach (var categoryName in categories)
            {
                var category = await _context.categories.FirstOrDefaultAsync(x => x.Name == categoryName);
                if(category != null)
                {
                    product.ProductCategories.Add(new ProductCategory { Category = category });
                }
                foreach (string? image in images)
                {
                    string filePath;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        filePath = $"{Environment.CurrentDirectory}\\Images\\products\\{image}";
                    }
                    else
                    {
                        filePath = $"{Environment.CurrentDirectory}/Images/products/{image}";
                    }

                    var fileBytes = File.ReadAllBytes(filePath);
                    var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                    product.ProductImages.Add(new ProductImage { Image = imagePath });
                }
                _context.Products.Add(product);

            }
        }
        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, string Adress, string image, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if(user == null)
            {
                var city = await _context.cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                city ??= await _context.cities.FirstOrDefaultAsync();
                var filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

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
                    Photo=imagePath,
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
                _context.categories.Add(new Category { Name = "Apple" });
                _context.categories.Add(new Category { Name = "Cars" });
                _context.categories.Add(new Category { Name = "Beauty" });
                _context.categories.Add(new Category { Name = "Footwear" });
                _context.categories.Add(new Category { Name = "Food" });
                _context.categories.Add(new Category { Name = "Cosmetics" });
                _context.categories.Add(new Category { Name = "Sports" });
                _context.categories.Add(new Category { Name = "Gaming" });
                _context.categories.Add(new Category { Name = "Toys" });
                _context.categories.Add(new Category { Name = "Pets" });
                _context.categories.Add(new Category { Name = "Nutrition" });
                _context.categories.Add(new Category { Name = "Clothing" });
                _context.categories.Add(new Category { Name = "Technology" });
            }
            await _context.SaveChangesAsync();
        }
    }
}
