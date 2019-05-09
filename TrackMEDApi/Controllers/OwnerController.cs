using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMEDApi.Models;
using System;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class OwnerController : ApiControllerCommon<Owner>
    {
        public OwnerController(IEntityRepository<Owner> entityService) 
                : base(entityService)
        {
        }

        // POST api/Owners
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Owner entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate description allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.Desc);
            if (result != null)
            {
                return Ok(); // already there
            }

            await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetOwner", new { controller = "Owner", id = entity.Id }, entity);
            return Ok();
        }
    }
}
