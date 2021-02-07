using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMEDApi.Core;
using TrackMEDApi.Models;
using TrackMEDApi.Services;
using System.Linq;
using Microsoft.CodeAnalysis;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class ComponentController : Controller
    {
        private readonly IEntityRepository<Component> _entityRepository;
        private readonly IEntityRepository<Description> _descriptionRepository;
        private readonly IEntityRepository<Owner> _ownerRepository;
        private readonly IEntityRepository<Model_Manufacturer> _modelmanufacturerRepository;
        private readonly IEntityRepository<ProviderOfService> _serviceproviderRepository;
        private readonly IEntityRepository<Status> _statusRepository;

        private readonly ILogger<ComponentController> _logger;
        private IEmailSender _mailService;

        public ComponentController(IEntityRepository<Component> entityRepository,
                                   IEntityRepository<Description> descriptionRepository,
                                   IEntityRepository<Owner> ownerRepository,
                                   IEntityRepository<Model_Manufacturer> modelmanufacturerRepository,
                                   IEntityRepository<ProviderOfService> serviceproviderRepository,
                                   IEntityRepository<Status> statusRepository,
                                   ILogger<ComponentController> logger,
                                   IEmailSender mailService)
        {
            _entityRepository = entityRepository;
            _descriptionRepository = descriptionRepository;
            _ownerRepository = ownerRepository;
            _modelmanufacturerRepository = modelmanufacturerRepository;
            _serviceproviderRepository = serviceproviderRepository;
            _statusRepository = statusRepository;

            _logger = logger;
            _mailService = mailService;
        }
       
        // GET: api/components
        [HttpGet]        
        public async Task<IEnumerable<Component>> GetAllAsync()
        {
            IEnumerable<Component> componentRecordsIE = await _entityRepository.GetAllAsync();
            return PopulateComponents(componentRecordsIE.ToList());
        }

        // GET api/components/5768d88b3d22c8953fc8007b
        [HttpGet("{id}", Name = "GetComponent")]
        public async Task<Component> GetOneAsync(string id)
        {
            Component c = await _entityRepository.GetOneAsync(id);
            c.Description = !String.IsNullOrEmpty(c.DescriptionID) ? _descriptionRepository.GetOneAsync(c.DescriptionID).Result : null;
            c.Owner = !String.IsNullOrEmpty(c.OwnerID) ? _ownerRepository.GetOneAsync(c.OwnerID).Result : null;
            c.Model_Manufacturer = !String.IsNullOrEmpty(c.Model_ManufacturerID) ? _modelmanufacturerRepository.GetOneAsync(c.Model_ManufacturerID).Result : null;
            c.ProviderOfService = !String.IsNullOrEmpty(c.ProviderOfServiceID) ? _serviceproviderRepository.GetOneAsync(c.ProviderOfServiceID).Result : null;
            c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
            return c;
        }

        /*
        [HttpGet]
        [Route("multiples/{ids}")]
        public async Task<IEnumerable<Component>> GetManyAsync(List<string> ids)
        {
            List<Component> components = new List<Component>();
            
            foreach( var id in ids)
            {
                var doc = await GetOneAsync(id);
                if (doc == null) continue;
                components.Add(doc);
            };

            return components;
        }
        */

        [HttpGet]
        [Route("{tableID}/{id}")]
        public async Task<IEnumerable<Component>> GetSelectedAsync(string tableID, string id)
        {
            IEnumerable<Component> componentRecordsIE = await _entityRepository.GetSelectedAsync(tableID, id);
            return PopulateComponents(componentRecordsIE.ToList());
        }

        // GET api/Component/{Description}
        [AllowAnonymous]
        [HttpGet]
        [Route("Desc/{Description}")]
        public async Task<Component> GetOneAsyncByDescription(string Description)
        {
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }

        // POST api/components
        // Content-Type: application/json 
        // Ex. { imte: "imte04", serialnumber: "ser04" }. MongoDB will automatically generate _id
        [HttpPost]
        //[Route("{entity}")]
        public async Task<IActionResult> PostOneAsync([FromBody]Component entity)
        {
            if (entity == null ) return BadRequest();

            // no duplicate imte allowed
            var result = await _entityRepository.GetOneAsyncByDescription(entity.imte);
            if (result != null)
            {
                return BadRequest();
            }

            await _entityRepository.AddOneAsync(entity);
            return CreatedAtRoute("GetComponent", new { controller = "Component", id = entity.Id }, entity);

            /* See http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            // Validate and add book to database (not shown)

            var response = Request.CreateResponse(HttpStatusCode.Created);

            // Generate a link to the new book and set the Location header in the response.
            string uri = Url.Link("GetBookById", new { id = book.BookId });
            response.Headers.Location = new Uri(uri);
            return response;
             */
        }

        // PUT api/components/5
        [HttpPut]
        public async Task<ReplaceOneResult> PutOneAsync([FromBody]Component entity)
        // public async Task<bool> PutOneAsync([FromBody]Component entity)
        {
            // nullify to prevent non-synchronization
            entity.Description = null;
            entity.Model_Manufacturer = null;
            entity.Owner = null;
            entity.ProviderOfService = null;
            entity.Status = null;

            return await _entityRepository.SaveOneAsync(entity);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
        }

        #region Private Helper Methods
        private List<Component> PopulateComponents(List<Component> componentRecordsIE)
        {
            List<Component> componentList = new List<Component>();
            foreach (Component c in componentRecordsIE)
            {
                c.Description = !String.IsNullOrEmpty(c.DescriptionID) ? _descriptionRepository.GetOneAsync(c.DescriptionID).Result : null;
                c.Owner = !String.IsNullOrEmpty(c.OwnerID) ? _ownerRepository.GetOneAsync(c.OwnerID).Result : null;
                c.Model_Manufacturer = !String.IsNullOrEmpty(c.Model_ManufacturerID) ? _modelmanufacturerRepository.GetOneAsync(c.Model_ManufacturerID).Result : null;
                c.ProviderOfService = !String.IsNullOrEmpty(c.ProviderOfServiceID) ? _serviceproviderRepository.GetOneAsync(c.ProviderOfServiceID).Result : null;
                c.Status = !String.IsNullOrEmpty(c.StatusID) ? _statusRepository.GetOneAsync(c.StatusID).Result : null;
                componentList.Add(c);

                // Comment out in production
                // if (componentList.Count > 99) break;
            }

            return componentList;
        }
        #endregion
    }
}
