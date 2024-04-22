using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StatesController : GenericController<State>
    {
        private readonly IStatesUnitOfWork _statesUnitOfWork;

        public StatesController(IGenericUnitOfWork<State> UnitOfWork , IStatesUnitOfWork statesUnitOfWork) : base(UnitOfWork)
        {
           _statesUnitOfWork = statesUnitOfWork;
        }
        [HttpGet("full")]
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
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _statesUnitOfWork.GetAsync(pagination);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _statesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

    }
}
