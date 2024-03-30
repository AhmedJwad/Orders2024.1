using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class CountriesRepository :GenericRepository<Country>, ICountriesRepository
    {
        private readonly DataContext _context;

        public CountriesRepository(DataContext context): base(context)
        { 
           _context = context;
        }
        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
            var countries=await _context.countries.Include(x=>x.states).ThenInclude(x=>x.cities).FirstOrDefaultAsync(x=>x.Id==id);
            if(countries == null)
            {
                return new ActionResponse<Country>
                {
                    wasSuccess = false,
                    Message = "country not exict",
                };
            }
            return new ActionResponse<Country>
            {
                wasSuccess = true,
                Result = countries,
            };

        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
        {
            var countries = await _context.countries.Include(x => x.states).ThenInclude(x=>x.cities).ToListAsync();
            return new ActionResponse<IEnumerable<Country>>
            {
                wasSuccess = true,
                Result = countries,
            };
        }
    }
}
