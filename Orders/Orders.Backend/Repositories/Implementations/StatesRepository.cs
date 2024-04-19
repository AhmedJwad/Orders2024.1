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
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly DataContext _context;

        public StatesRepository(DataContext context) : base(context)
        {
           _context = context;
        }
        [HttpGet]
        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        {
            var state=await _context.states.OrderBy(x=>x.Name).ToListAsync(); 
            return new ActionResponse<IEnumerable<State>>
            {
              wasSuccess=true,
              Result=state,
            };
        }
        
        public override async Task<ActionResponse<State>> GetAsync(int Id)
        {
            var state = await _context.states.Include(x => x.cities).FirstOrDefaultAsync(x => x.Id == Id);
            if(state==null)
            {
                return new ActionResponse<State>
                {
                    wasSuccess = false,
                    Message = "state not found"
                };

            }
            return new ActionResponse<State>
            {
                wasSuccess = true,
                Result = state,
            };
        }
        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.states.Include(x => x.cities).Where(x => x.Country!.Id == pagination.Id).AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return new ActionResponse<IEnumerable<State>>
            {
                wasSuccess = true,
                Result = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable=_context.states.Where(x=>x.Country!.Id==pagination.Id).AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPage = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                wasSuccess = true,
                Result=totalPage,
            };
        }
    }
}
