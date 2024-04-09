﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : GenericController<Category>
    {
        private readonly ICategoriesUnitOfWork _categoriesUnitOfWork;

        public CategoriesController(IGenericUnitOfWork<Category> unit, ICategoriesUnitOfWork categoriesUnitOfWork) : base(unit)
        {
           _categoriesUnitOfWork = categoriesUnitOfWork;
        }
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery]PaginationDTO pagination)
        {
            var response=await _categoriesUnitOfWork.GetAsync(pagination);
            if(response.wasSuccess)
            {
                return Ok (response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _categoriesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.wasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();

        }
    }
}