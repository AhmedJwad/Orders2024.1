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
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductsController : GenericController<Product>
    {
        private readonly IProductsUnitOfWork _productsUnitOfWork;

        public ProductsController(IGenericUnitOfWork<Product> UnitOfWork, 
            IProductsUnitOfWork productsUnitOfWork):base(UnitOfWork)
        {
           _productsUnitOfWork = productsUnitOfWork;
        }
        [AllowAnonymous]
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery]PaginationDTO paginationDTO)
        {
            var repostry=await _productsUnitOfWork.GetAsync(paginationDTO);
            if(repostry.wasSuccess)
            {
                return Ok(repostry.Result);
            }
            return BadRequest();

        }
        [AllowAnonymous]
        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery]PaginationDTO pagination)
        {
            var rsponse=await _productsUnitOfWork.GetTotalPagesAsync(pagination);
            if(rsponse.wasSuccess)
            {
                return Ok(rsponse.Result);
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response=await _productsUnitOfWork.GetAsync(id);
            if(response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
        [HttpPost("full")]
        public  async Task<IActionResult> PostFullAsync(ProductDTO productDTO)
        {
            var response=await _productsUnitOfWork.AddFullAsync(productDTO);
            if(response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
        [HttpPut("full")]
        public async Task<IActionResult> PutFullAsync(ProductDTO product)
        {
            var action = await _productsUnitOfWork.UpdateFullAsync(product);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPost("addImages")]
        public async Task<IActionResult> PostAddImagesAsync (ImageDTO imageDTO)
        {
            var action=await _productsUnitOfWork.AddImageAsync(imageDTO);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);

        }
        [HttpPost("removeLastImage")]
        public async Task<IActionResult> PostRemoveLastImageAsync(ImageDTO imageDTO)
        {
            var action = await _productsUnitOfWork.RemoveLastImageAsync(imageDTO);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }
        [HttpDelete("{id}")]
        public override async Task<IActionResult> DeleteAsync(int id)
        {
            var action = await _productsUnitOfWork.DeleteAsync(id);
            if (!action.wasSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
