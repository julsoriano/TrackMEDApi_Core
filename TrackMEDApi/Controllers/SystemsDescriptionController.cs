using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class SystemsDescriptionController : ApiControllerCommon<SystemsDescription>
    {
        public SystemsDescriptionController(IEntityRepository<SystemsDescription> entityService) 
                : base(entityService)
        {
        }

        // POST api/SystemsDescription
        [HttpPost]
        //[Route("{entity}")]
        public async Task<IActionResult> PostOneAsync([FromBody]SystemsDescription entity)
        {
            if (entity == null ) return BadRequest();

            // no duplicate SystemsDescription allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return Ok(); // already there
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetSystemsDescription", new { controller = "SystemsDescription", id = entity.Id }, entity);
            return Ok();
        }
    }
}
