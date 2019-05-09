using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMEDApi.Models;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class EquipmentActivityController : Controller
    {
        private readonly IEntityRepository<EquipmentActivity> _entityRepository;
        private readonly IEntityRepository<ActivityType> _activitytypeRepository;
        private readonly IEntityRepository<Component> _componentRepository;
        private readonly IEntityRepository<Deployment> _deploymentRepository;
        private readonly IEntityRepository<ProviderOfService> _serviceproviderRepository;
        private readonly IEntityRepository<Status> _statusRepository;
        private readonly IEntityRepository<SystemTab> _systemtabRepository;

        public EquipmentActivityController(IEntityRepository<EquipmentActivity> entityRepository, 
                                           IEntityRepository<ActivityType> activitytypeRepository,
                                           IEntityRepository<Component> componentRepository,
                                           IEntityRepository<Deployment> deploymentRepository,
                                           IEntityRepository<ProviderOfService> serviceproviderRepository,
                                           IEntityRepository<Status> statusRepository, 
                                           IEntityRepository<SystemTab> systemtabRepository)
        {
            _entityRepository = entityRepository;
            _activitytypeRepository = activitytypeRepository;
            _componentRepository = componentRepository;
            _deploymentRepository = deploymentRepository;
            _serviceproviderRepository = serviceproviderRepository;
            _statusRepository = statusRepository;
            _systemtabRepository = systemtabRepository;
        }

        // GET: api/EquipmentActivities
        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<EquipmentActivity>> GetAllAsync(string id = null)
        {
            var recSelectedIE = await _entityRepository.GetAllAsync();
            if (id != null) recSelectedIE = recSelectedIE.Where(s => s.DeploymentID == id);
 
            return PopulateRecords(recSelectedIE.ToList());
        }
        /*
        // GET api/Categories/5768d88b3d22c8953fc8007b
        [HttpGet("{id}", Name = "GetEquipmentActivity")]
        public async Task<EquipmentActivity> GetOneAsync(string id)
        {
            EquipmentActivity c = await _entityRepository.GetOneAsync(id);
            c.ActivityType = !String.IsNullOrEmpty(c.ActivityTypeID) ? _activitytypeRepository.GetOneAsync(c.ActivityTypeID).Result : null;
            c.ServiceProvider = !String.IsNullOrEmpty(c.ServiceProviderID) ? _serviceproviderRepository.GetOneAsync(c.ServiceProviderID).Result : null;
            c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
            return c;
        }
        */

        [HttpGet]
        [Route("{tableID}/{id}")]
        public async Task<IEnumerable<EquipmentActivity>> GetSelectedAsync(string tableID, string id)
        {
            // get component with id == imte
            Component c = _componentRepository.GetOneAsyncByDescription(id).Result;

            // use ObjectId of c to get all equipment activity records with that ObjectId
            IEnumerable<EquipmentActivity> eqactRecordsIE = await _entityRepository.GetSelectedAsync(tableID, c.Id);
            return PopulateRecords(eqactRecordsIE.ToList());
        }

        // GET api/EquipmentActivitys/{Description}
        [AllowAnonymous]
        [HttpGet]
        [Route("Desc/{Description}")]
        public async Task<EquipmentActivity> GetOneAsyncByDescription(string Description)
        {
            // http://stackoverflow.com/questions/1405048/how-do-i-decode-a-url-parameter-using-c
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }

        // POST api/EquipmentActivitys
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]EquipmentActivity entity)
        {
            if (entity == null) return BadRequest();

            var result = await _entityRepository.AddOneAsync(entity);
            //return CreatedAtRoute("GetEquipmentActivity", new { controller = "EquipmentActivity", id = entity.Id }, entity);
            return Ok();
        }

        // PUT api/EquipmentActivitys/5
        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody]EquipmentActivity entity)
        {
            return await _entityRepository.SaveOneAsync(entity);
        }

        // DELETE api/EquipmentActivitys/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
        }

        #region Private Helper Methods
        private List<EquipmentActivity> PopulateRecords(List<EquipmentActivity> componentRecordsIE)
        {
            List<EquipmentActivity> recordList = new List<EquipmentActivity>();
            foreach (EquipmentActivity eqact in componentRecordsIE)
            {
                eqact.ActivityType = !String.IsNullOrEmpty(eqact.ActivityTypeID) ? _activitytypeRepository.GetOneAsync(eqact.ActivityTypeID).Result : null;
                eqact.ProviderOfService = !String.IsNullOrEmpty(eqact.ProviderOfServiceID) ? _serviceproviderRepository.GetOneAsync(eqact.ProviderOfServiceID).Result : null;
                eqact.Status = !String.IsNullOrEmpty(eqact.StatusID) ? _statusRepository.GetOneAsync(eqact.StatusID).Result : null;
                recordList.Add(eqact);
            }

            return recordList;
        }
        #endregion
    }
}
