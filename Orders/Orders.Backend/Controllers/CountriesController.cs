﻿using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitOfWork _countriesUnitOfWork;

        public CountriesController(IGenericUnitOfWork<Country> UnitOfWork, ICountriesUnitOfWork countriesUnitOfWork) : base(UnitOfWork)
        {
            _countriesUnitOfWork = countriesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _countriesUnitOfWork.GetAsync();
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _countriesUnitOfWork.GetAsync(id);
            if (response.wasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}
