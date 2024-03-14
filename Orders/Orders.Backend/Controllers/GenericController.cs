using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;

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

        [HttpGet]
        public virtual async Task<ActionResult> GetAsync()
        {
            var action=await _genericUnitOfWork.GetAsync();
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
        [HttpGet("id")]
        public virtual async Task<ActionResult>GetAsync(int id)
        {
            var action = await _genericUnitOfWork.GetAsync(id);
            if(action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound();
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
        public virtual async Task<ActionResult>DeleteAsync(int id)
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
