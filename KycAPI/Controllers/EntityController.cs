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

        [HttpGet("bycountry/{country}")]
        public async Task<IActionResult> GetEntitiesByCountry(string country)
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e => e.Addresses.Any(a => a.Country == country))
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("byaddressline/{addressLine}")]
        public async Task<IActionResult> GetEntitiesByAddressLine(string addressLine)
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e => e.Addresses.Any(a => a.AddressLine.Contains(addressLine)))
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("byfirstname/{firstName}")]
        public async Task<IActionResult> GetEntitiesByFirstName(string firstName)
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e => e.Names.Any(n => n.FirstName.Contains(firstName)))
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("bymiddlename/{middleName}")]
        public async Task<IActionResult> GetEntitiesByMiddleName(string middleName)
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e => e.Names.Any(n => n.MiddleName.Contains(middleName)))
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpGet("bysurname/{surname}")]
        public async Task<IActionResult> GetEntitiesBySurname(string surname)
        {
            var entities = await dbContext.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .Where(e => e.Names.Any(n => n.Surname.Contains(surname)))
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }

            return Ok(entities);
        }

    }
}
