using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : ApiControllerCommon<Location>
    {
        public LocationController(IEntityRepository<Location> entityService) 
                : base(entityService)
        {
        }

        // POST api/Locations
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Location entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return Ok(); // already there
            }

            await _entityRepository.AddOneAsync(entity);
            return Ok();
            // return CreatedAtRoute("GetLocation", new { controller = "Location", id = entity.Id }, entity);
        }
    }
}
