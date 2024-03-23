

using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
           _context = context;
        }

        public async Task SeedAsync()
        {
           await _context.Database.EnsureCreatedAsync();
           await CheckCountriesAsync();
           await CheckCategoriesAsync();

        }

       

        private async Task CheckCountriesAsync()
        {
            if(!_context.countries.Any())
            {
                _context.countries.Add(new Country { Name = "Iraq" });
                _context.countries.Add(new Country { Name = "Oman" });
            }
            await _context.SaveChangesAsync();  
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
