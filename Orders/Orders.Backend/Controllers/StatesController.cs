using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : GenericController<State>
    {
        private readonly IStatesUnitOfWork _statesUnitOfWork;

        public StatesController(IGenericUnitOfWork<State> UnitOfWork , IStatesUnitOfWork statesUnitOfWork) : base(UnitOfWork)
        {
           _statesUnitOfWork = statesUnitOfWork;
        }
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response=await _statesUnitOfWork.GetAsync();
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
        [HttpGet("Id")]
        public override async Task<IActionResult> GetAsync(int Id)
        {
            var response = await _statesUnitOfWork.GetAsync(Id);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}
