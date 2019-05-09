using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class DescriptionController : ApiControllerCommon<Description>
    { 
        public DescriptionController(IEntityRepository<Description> entityService) 
                : base(entityService)
        {
        }

        // POST api/Descriptions
        [HttpPost]
        //[Route("{entity}")]
        public async Task<IActionResult> AddOneAsync([FromBody]Description entity)
        {
            if (entity == null ) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return BadRequest();
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetDescription", new { controller = "Description", id = entity.Id }, entity);
            return Ok();
        }

        /*
        // POST api/Descriptions/entities
        [HttpPost]
        [Route("multiples/{entities}")]
        public async Task<bool> PostManyAsync([FromBody]List<Description> entities)
        {
            foreach (var entity in entities)
            {
                var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
                if (result == null)
                {
                    await _entityRepository.AddOneAsync(entity);
                }
            }
            return true;
        }
        */
    }
}
