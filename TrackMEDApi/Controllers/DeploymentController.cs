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
    public class DeploymentController : Controller
    {
        private readonly IEntityRepository<Deployment> _entityRepository;
        private readonly IEntityRepository<Location> _locationRepository;

        public DeploymentController(IEntityRepository<Deployment> entityRepository, IEntityRepository<Location> locationRepository)
        {
            _entityRepository = entityRepository;
            _locationRepository = locationRepository;
        }

        // GET: api/Deployments
        [HttpGet]
        public async Task<IEnumerable<Deployment>> GetAllAsync()
        {
            return await _entityRepository.GetAllAsync();
        }

        // GET api/Categories/5768d88b3d22c8953fc8007b
        [HttpGet("{id}", Name = "GetDeployment")]
        public async Task<Deployment> GetOneAsync(string id)
        {
            return await _entityRepository.GetOneAsync(id);
        }

        // GET api/Deployments/{Description}
        [AllowAnonymous]
        [HttpGet]
        [Route("Desc/{Description}")]
        public async Task<Deployment> GetOneAsyncByDescription(string Description)
        {
            // http://stackoverflow.com/questions/1405048/how-do-i-decode-a-url-parameter-using-c
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }
        
        [HttpGet]
        [Route("{fieldID}/{id}/{tableID}")] // same signature as GetSelectedAsync
        public async Task<Deployment> GetOneAsyncByFieldID(string fieldID, string id, string tableID)
        {
            return await _entityRepository.GetOneAsyncByFieldID(fieldID, id);
        }
        
        [HttpGet]
        [Route("{tableID}/{id}")] // http://tutlane.com/tutorial/aspnet-mvc/url-routing-in-asp-net-mvc-example-with-multiple-parameters-query-strings
        public async Task<IEnumerable<Deployment>> GetSelectedAsync(string tableID, string id)
        {
            IEnumerable<Deployment> recIE = await _entityRepository.GetSelectedAsync(tableID, id);
            return PopulateRecords(recIE.ToList());
        }

        // POST api/Deployments
        // Content-Type: application/json 
        [HttpPost]
        public async Task<IActionResult> AddOneAsync([FromBody]Deployment entity)
        {
            if (entity == null) return BadRequest();

            // no duplicate id allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.DeploymentID);
            if (result != null)
            {
                return BadRequest();
            }

            await _entityRepository.AddOneAsync(entity);
            return CreatedAtRoute("GetDeployment", new { controller = "Deployment", id = entity.Id }, entity);
        }

        // PUT api/Deployments/5
        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody]Deployment entity)
        {
            return await _entityRepository.SaveOneAsync(entity);
        }

        // DELETE api/Deployments/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
        }

        #region Private Helper Methods
        private List<Deployment> PopulateRecords(List<Deployment> componentRecordsIE)
        {
            List<Deployment> recordList = new List<Deployment>();
            foreach (Deployment rec in componentRecordsIE)
            {
                rec.Location = !String.IsNullOrEmpty(rec.LocationID) ? _locationRepository.GetOneAsync(rec.LocationID).Result : null;
                recordList.Add(rec);
            }

            return recordList;
        }
        #endregion
    }
}
