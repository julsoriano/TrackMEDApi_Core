using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : ApiControllerCommon<Status>
    {
        public StatusController(IEntityRepository<Status> entityService) 
                : base(entityService)
        {
        }

        // POST api/Categories
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Status entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return Ok(); // already there
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetStatus", new { controller = "Status", id = entity.Id }, entity);
            return Ok();
        }
    }
}
