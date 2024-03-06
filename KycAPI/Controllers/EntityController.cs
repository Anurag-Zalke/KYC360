using KycAPI.Data;
using KycAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KycAPI.Controllers
{
    [ApiController]
    [Route("api/entities")]
    public class EntityController : ControllerBase
    {
        private readonly KycAPIDbContext dbContext;

        public EntityController(KycAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity()
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .ToListAsync();

            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntity(string id)
        {
            var entity = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .FirstOrDefaultAsync(e => e.Id == id);


            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> AddEntity(AddEntityRequest addEntityRequest)
        {
            var entity = new Entity()
            {
                Id = Guid.NewGuid().ToString(),
                Addresses = addEntityRequest.Addresses,
                Dates = addEntityRequest.Dates,
                Deceased = addEntityRequest.Deceased,
                Gender = addEntityRequest.Gender,
                Names = addEntityRequest.Names
            };

            await dbContext.Entities.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return Ok(entity);  
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity(string id, AddEntityRequest addEntityRequest)
        {
            var entity = await dbContext.Entities.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Addresses = addEntityRequest.Addresses;
            entity.Dates = addEntityRequest.Dates;
            entity.Deceased = addEntityRequest.Deceased;
            entity.Gender = addEntityRequest.Gender;
            entity.Names = addEntityRequest.Names;

            await dbContext.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(string id)
        {
            var entity = await dbContext.Entities.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            dbContext.Entities.Remove(entity);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEntities([FromQuery] EntitySearchParameters searchParameters)
        {
            IQueryable<Entity> query = dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names);

            if (!string.IsNullOrWhiteSpace(searchParameters.Country))
            {
                query = query.Where(e => e.Addresses.Any(a => a.Country.Contains(searchParameters.Country)));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.AddressLine))
            {
                query = query.Where(e => e.Addresses.Any(a => a.AddressLine.Contains(searchParameters.AddressLine)));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.FirstName))
            {
                query = query.Where(e => e.Names.Any(n => n.FirstName.Contains(searchParameters.FirstName)));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.MiddleName))
            {
                query = query.Where(e => e.Names.Any(n => n.MiddleName.Contains(searchParameters.MiddleName)));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.Surname))
            {
                query = query.Where(e => e.Names.Any(n => n.Surname.Contains(searchParameters.Surname)));
            }

            var entities = await query.ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("searchtext")]
        public async Task<IActionResult> SearchEntities([FromQuery] string? searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return BadRequest("Search text is required.");
            }

            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e =>
                    e.Addresses.Any(a => a.Country.Contains(searchText)) ||
                    e.Addresses.Any(a => a.AddressLine.Contains(searchText)) ||
                    e.Names.Any(n => n.FirstName.Contains(searchText)) ||
                    e.Names.Any(n => n.MiddleName.Contains(searchText)) ||
                    e.Names.Any(n => n.Surname.Contains(searchText))
                )
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("AdvanceSearch")]
        public async Task<IActionResult> SearchEntities([FromQuery] string? gender,
                                                        [FromQuery] DateTime? startDate,
                                                        [FromQuery] DateTime? endDate,
                                                        [FromQuery] List<string>? countries)
        {
            IQueryable<Entity> query = dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names);


            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(e => e.Gender == gender);
            }

            if (startDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d.DateValue >= startDate));
            }

            if (endDate != null)
            {
                endDate = endDate.Value.AddDays(1).Date;
                query = query.Where(e => e.Dates.Any(d => d.DateValue <= endDate));
            }

            if (countries != null && countries.Any())
            {
                query = query.Where(e => e.Addresses.Any(a => countries.Contains(a.Country)));
            }

            var entities = await query.ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }
    }
}
