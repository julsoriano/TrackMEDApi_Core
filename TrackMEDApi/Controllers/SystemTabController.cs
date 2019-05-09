using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackMEDApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class SystemTabController : Controller
    {
        private readonly IEntityRepository<SystemTab> _entityRepository;
        private readonly IEntityRepository<Component> _componentRepository;
        private readonly IEntityRepository<Location> _locationRepository;
        private readonly IEntityRepository<Owner> _ownerRepository;
        private readonly IEntityRepository<Status> _statusRepository;
        private readonly IEntityRepository<SystemsDescription> _systemsdescriptionRepository;

        public SystemTabController(IEntityRepository<SystemTab> entityRepository, 
                                   IEntityRepository<Component> componentRepository,
                                   IEntityRepository<Location> locationRepository,
                                   IEntityRepository<Owner> ownerRepository,
                                   IEntityRepository<Status> statusRepository,
                                   IEntityRepository<SystemsDescription> systemsdescriptionRepository)
        {
            _entityRepository = entityRepository;
            _componentRepository = componentRepository;
            _locationRepository = locationRepository;
            _ownerRepository = ownerRepository;
            _statusRepository = statusRepository;
            _systemsdescriptionRepository = systemsdescriptionRepository;
        }

        // GET: api/systemtabs
        [HttpGet]        
        public async Task<IEnumerable<SystemTab>> GetAllAsync()
        {
            IEnumerable<SystemTab> systemRecordsIE = await _entityRepository.GetAllAsync();
            systemRecordsIE = systemRecordsIE.OrderBy(s => s.imte);

            List<SystemTab> systemList = new List<SystemTab>();
            foreach (SystemTab c in systemRecordsIE)
            {
                c.SystemsDescription = !String.IsNullOrEmpty(c.SystemsDescriptionID) ? _systemsdescriptionRepository.GetOneAsync(c.SystemsDescriptionID).Result : null;
                c.Owner = !String.IsNullOrEmpty(c.OwnerID) ? _ownerRepository.GetOneAsync(c.OwnerID).Result : null;
                c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
                c.Location = !String.IsNullOrEmpty(c.LocationID) ? _locationRepository.GetOneAsync(c.LocationID).Result : null;
                systemList.Add(c);
            }

            return systemList;
        }

        // GET api/systemtabs/5768d88b3d22c8953fc8007b
        // {"Id":"5768d88b3d22c8953fc8007b","imte":"imte01","serialnumber":"ser01","Notes":null,"RowVersion":null} 
        [HttpGet("{id}", Name = "GetSystemTab")]
        public async Task<SystemTab> GetOneAsync(string id)
        {
            SystemTab c = await _entityRepository.GetOneAsync(id);
            c.SystemsDescription = !String.IsNullOrEmpty(c.SystemsDescriptionID) ? _systemsdescriptionRepository.GetOneAsync(c.SystemsDescriptionID).Result : null;
            c.Owner = !String.IsNullOrEmpty(c.OwnerID) ? _ownerRepository.GetOneAsync(c.OwnerID).Result : null;
            c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
            c.Location = !String.IsNullOrEmpty(c.LocationID) ? _locationRepository.GetOneAsync(c.LocationID).Result : null;
            return c;
        }

        // GET api/SystemsDescriptions/{Description}
        [AllowAnonymous]
        [HttpGet]
        [Route("Desc/{Description}")]
        public async Task<SystemTab> GetOneAsyncByDescription(string Description)
        {
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }

        [HttpGet]
        [Route("{tableID}/{id}")]
        public async Task<IEnumerable<SystemTab>> GetSelectedAsync(string tableID, string id)
        {
            IEnumerable<SystemTab> componentRecordsIE = await _entityRepository.GetSelectedAsync(tableID, id);
            return PopulateComponents(componentRecordsIE.ToList());
        }

        // POST api/systemtabs
        // Content-Type: application/json 
        // Ex. { imte: "imte04", serialnumber: "ser04" }. MongoDB will automatically generate _id
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]SystemTab entity)
        {
            if (entity == null ) return BadRequest();

            // no duplicate imte allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.imte);
            if (result != null)
            {
                return BadRequest();
            }

            await _entityRepository.AddOneAsync(entity);

            return CreatedAtRoute("GetSystemTab", new { controller = "SystemTab", id = entity.Id }, entity);
        }

        // PUT api/systemtabs/5
        [HttpPut]
        public async Task<bool> Put([FromBody]SystemTab entity)
        {
            return await _entityRepository.SaveOneAsync(entity);
        }

        // DELETE api/systemtabs/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
        }

        #region Private Helper Methods
        private List<SystemTab> PopulateComponents(List<SystemTab> componentRecordsIE)
        {
            List<SystemTab> componentList = new List<SystemTab>();
            foreach (SystemTab c in componentRecordsIE)
            {
                c.SystemsDescription = !String.IsNullOrEmpty(c.SystemsDescriptionID) ? _systemsdescriptionRepository.GetOneAsync(c.SystemsDescriptionID).Result : null;
                c.Owner = !String.IsNullOrEmpty(c.OwnerID) ? _ownerRepository.GetOneAsync(c.OwnerID).Result : null;
                c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
                c.Location = !String.IsNullOrEmpty(c.LocationID) ? _locationRepository.GetOneAsync(c.LocationID).Result : null;
                componentList.Add(c);
            }

            return componentList;
        }
        #endregion
    }
}
