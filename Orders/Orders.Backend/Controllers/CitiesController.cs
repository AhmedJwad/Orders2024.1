using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : GenericController<City>
    {
        public CitiesController(IGenericUnitOfWork<City> UnitOfWork) : base(UnitOfWork)
        {

        }
    }
}
