using Microsoft.AspNetCore.Mvc;
using HomeEnergyApi.Models;
using Microsoft.EntityFrameworkCore;
using HomeEnergyApi.Pagination;

namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomesController : ControllerBase
    {
        private IPaginatedReadRepository<int, Home> paginatedReadRepository;

        public HomesController(IPaginatedReadRepository<int, Home> paginatedReadRepository)
        {

            this.paginatedReadRepository = paginatedReadRepository;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? ownerLastName,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            PaginatedResult<Home> paginatedResult;

            if (ownerLastName != null)
            {
                paginatedResult = paginatedReadRepository.FindPaginatedByOwnerLastName((string)ownerLastName, pageNumber, pageSize);
            }
            else
            {
                paginatedResult = paginatedReadRepository.FindPaginated(pageNumber, pageSize);
            }

            var nextPageUrl = pageNumber < paginatedResult.TotalPages
                ? Url.Action(nameof(Get), new { pageNumber = pageNumber + 1, pageSize })
                : null;

            return Ok(new
            {
                Homes = paginatedResult.Items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = paginatedResult.TotalCount,
                TotalPages = paginatedResult.TotalPages,
                NextPage = nextPageUrl
            });
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            if (id > paginatedReadRepository.FindAll().Count)
            {
                return NotFound();
            }
            var home = paginatedReadRepository.FindById(id);

            return Ok(home);
        }

        [HttpGet("Bang")]
        public IActionResult Bang()
        {
            throw new InvalidOperationException("You caused a loud bang.");
        }
    }
}