using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class Model_ManufacturerController : ApiControllerCommon<Model_Manufacturer>
    {
        public Model_ManufacturerController(IEntityRepository<Model_Manufacturer> entityService) 
                : base(entityService)
        {
        }

        // POST api/Model_Manufacturers
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Model_Manufacturer entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return Ok();  // already there
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetModel_Manufacturer", new { controller = "Model_Manufacturer", id = entity.Id }, entity);
            return Ok();
        }
    }
}
