using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TemporalOrders : GenericController<TemporalOrders>
    {
        private readonly ITemporalOrdersUnitOfWork _temporalOrdersUnitOfWork;

        public TemporalOrders(IGenericUnitOfWork<TemporalOrders> genericUnitOfWork ,
            ITemporalOrdersUnitOfWork temporalOrdersUnitOfWork) : base(genericUnitOfWork)
        {
            _temporalOrdersUnitOfWork = temporalOrdersUnitOfWork;
        }
        [HttpPost("full")]
        public async Task<IActionResult> PostAsync(TemporalOrderDTO temporalOrderDTO)
        {
            var action = await _temporalOrdersUnitOfWork.AddFullAsync(User.Identity!.Name!, temporalOrderDTO);
            if (action.wasSuccess)
            { 
                return Ok(action.Result);
            }

            return BadRequest(action.Message);
        }
        [HttpGet("my")]
        public override async Task<IActionResult> GetAsync()
        {
            var action=await _temporalOrdersUnitOfWork.GetAsync(User.Identity!.Name!);
            if (action.wasSuccess) 
            { return Ok(action.Result); }

            return BadRequest(action.Result);
        }

        [HttpGet("count")]

        public async Task<IActionResult> GetCountAsync()
        {
            var action = await _temporalOrdersUnitOfWork.GetCountAsync(User.Identity!.Name!);
            if (action.wasSuccess) { return Ok(action.Result); }
            return BadRequest(action.Result);
        }

    }
}
