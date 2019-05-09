using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class ActivityTypeController : ApiControllerCommon<ActivityType>
    {
        public ActivityTypeController(IEntityRepository<ActivityType> entityService) 
                : base(entityService)
        {
        }

        // POST api/ActivityTypes
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]ActivityType entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return BadRequest();
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetActivityType", new { controller = "ActivityType", id = entity.Id }, entity);
            return Ok();
        }
    }
}
