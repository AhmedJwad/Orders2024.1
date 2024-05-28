using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : Controller where T : class
    {
        private readonly IGenericUnitOfWork<T> _genericUnitOfWork;

        public GenericController(IGenericUnitOfWork<T> genericUnitOfWork)
        {
            _genericUnitOfWork = genericUnitOfWork;
        }

        [HttpGet("full")]
        public virtual async Task<IActionResult> GetAsync()
        {
            var action=await _genericUnitOfWork.GetAsync();
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
        [HttpGet("id")]
        public virtual async Task<IActionResult>GetAsync(int id)
        {
            var action = await _genericUnitOfWork.GetAsync(id);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound();
        }
        [HttpGet]
        public virtual async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _genericUnitOfWork.GetAsync(pagination);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
        [HttpGet("totalPages")]
        public virtual async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _genericUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpPost]
        public virtual async Task<ActionResult>AddAsync(T model)
        {
            var action=await _genericUnitOfWork.AddAsync(model);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpPut]
        public virtual async Task<ActionResult>updateAsync(T model)
        {
            var action=await _genericUnitOfWork.UpdateAsync(model);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpDelete("id")]
        public virtual async Task<IActionResult>DeleteAsync(int id)
        {
            var action=await _genericUnitOfWork.DeleteAsync(id);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }
    }
}
