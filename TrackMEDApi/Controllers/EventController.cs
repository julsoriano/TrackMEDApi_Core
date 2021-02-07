using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackMEDApi.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEntityRepository<Event> _entityRepository;

        public EventController(IEntityRepository<Event> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _entityRepository.GetAllAsync();
        }

        // GET api/Events/5768d88b3d22c8953fc8007b
        [HttpGet("{id}", Name = "GetEvent")]
        public async Task<Event> GetOneAsync(string id)
        {
            return await _entityRepository.GetOneAsync(id);
        }

        // GET api/Events/{Description}
        [AllowAnonymous]
        [HttpGet]
        [Route("{Description}")]
        public async Task<Event> GetOneAsyncByDescription(string Description)
        {
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }


        // POST api/Events
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Event entity)
        {
            if (entity == null) return BadRequest();

            await _entityRepository.AddOneAsync(entity);
            //return true;
            return CreatedAtRoute("GetEvent", new { controller = "Event", id = entity.Id }, entity);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ReplaceOneResult> Put([FromBody]Event entity)
        // public async Task<bool> Put([FromBody]Event entity)
        {
            return await _entityRepository.SaveOneAsync(entity);
        }

        /*
                [HttpPut("{id}")]
        public void Put(string id, [FromBody]Event entity)
        {
        } 
        */

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
            // return GetOneAsync(id) == null;
        }
    }
}
