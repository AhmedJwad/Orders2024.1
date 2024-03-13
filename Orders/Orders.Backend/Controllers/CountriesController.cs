using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult>GetAsync()
        {
            return Ok(await _context.countries.AsNoTracking().ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetAsync(int id)
        {
            var country = await _context.countries.FirstOrDefaultAsync(x => x.Id == id);
            if(country ==null)
            {
                return null;
            }
            return Ok(country);
        }
        [HttpPost]
        public async Task<IActionResult>PostAync(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return Ok(country);
        }
        [HttpDelete("id")]
        public async Task<IActionResult>DeleteAsync(int id)
        {
            var country = await _context.countries.FirstOrDefaultAsync(x=>x.Id==id);
            if(country == null) { return NotFound(); }

            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult>PutAsync(Country country)
        {
            _context.Update(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
