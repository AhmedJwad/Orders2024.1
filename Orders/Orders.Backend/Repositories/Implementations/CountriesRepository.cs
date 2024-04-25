using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
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

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.countries
                .Include(c => c.states)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return new ActionResponse<IEnumerable<Country>>
            {
                wasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Country>> GetComboAsync()
        {
            return await _context.countries.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.countries.AsQueryable();
            if(!string.IsNullOrWhiteSpace(pagination.Filter))
            {
               queryable=queryable.Where(x=>x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count=await queryable.CountAsync();
            int totalPage = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                wasSuccess=true,
                Result = totalPage,
            };
        }
    }
}
