using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.Helpers;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersHelper _ordersHelper;
        private readonly IOrdersUnitOfWork _ordersUnitOfWork;

        public OrdersController(IOrdersHelper ordersHelper, IOrdersUnitOfWork ordersUnitOfWork)
        {
            _ordersHelper = ordersHelper;
           _ordersUnitOfWork = ordersUnitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(OrderDTO orderDTO)
        {
            var response = await _ordersHelper.ProcessOrderAsync(User.Identity!.Name!, orderDTO.Remarks);
            if(response.wasSuccess)
            {
                return NoContent();
            }
            return BadRequest(response.Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _ordersUnitOfWork.GetAsync(User.Identity!.Name!, pagination);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _ordersUnitOfWork.GetTotalPagesAsync(User.Identity!.Name!, pagination);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _ordersUnitOfWork.GetAsync(id);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(OrderDTO orderDTO)
        {
            var response = await _ordersUnitOfWork.UpdateFullAsync(User.Identity!.Name!, orderDTO);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }


    }
}
