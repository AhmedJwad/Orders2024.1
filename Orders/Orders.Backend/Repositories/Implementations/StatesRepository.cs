using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
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
            var state=await _context.states.Include(x=>x.cities).ToListAsync(); 
            return new ActionResponse<IEnumerable<State>>
            {
              wasSuccess=true,
              Result=state,
            };
        }
        [HttpGet("id:int")]
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

    }
}
