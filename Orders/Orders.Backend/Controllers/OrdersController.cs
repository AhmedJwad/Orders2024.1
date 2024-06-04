using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.Helpers;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersHelper _ordersHelper;

        public OrdersController(IOrdersHelper ordersHelper)
        {
            _ordersHelper = ordersHelper;
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
    }
}
