using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiControllerCommon<T> : Controller where T : IEntity
    {
        internal readonly IEntityRepository<T> _entityRepository;

        public ApiControllerCommon(IEntityRepository<T> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        // GET: api/values
        // Building Your First Web API with ASP.NET Core MVC and Visual Studio https://docs.asp.net/en/latest/tutorials/first-web-api.html 
        /* Return values
        The GetAllAsync method returns a CLR object. MVC automatically serializes the object to JSON and writes the JSON into the body of the response message.
        The response code for this method is 200, assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)
        */
        [HttpGet]
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // return await _entityRepository.GetAllAsync();
            
            var allRecords = await _entityRepository.GetAllAsync();
            //var items = allRecords
            //             .OrderBy(x => x.Desc);
            return allRecords;       
        }

        // GET api/values/5
        /*
        In contrast, the GetOne method returns the more general IActionResult type, which represents a generic result type. That’s because GetById has two different return types:

        If no item matches the requested ID, the method returns a 404 error. This is done by returning NotFound.
        Otherwise, the method returns 200 with a JSON response body. This is done by returning an ObjectResult.         
        */
        [HttpGet("{id}")]
        public async Task<T> GetOneAsync(string id)
        {
            return await _entityRepository.GetOneAsync(id);
        }

        // GET api/values/{Description}
        /*
        {Description} will, at times, contains url-encoded characters and will be decoded in
        _entityRepository.GetOneAsyncByDescription(Description)
        */
        [AllowAnonymous]
        [HttpGet]
        [Route("Desc/{Description}")]
        public async Task<T> GetOneAsyncByDescription(string Description)
        {
            return await _entityRepository.GetOneAsyncByDescription(Description);
        }

        public async Task<ReplaceOneResult> Put([FromBody]T entity)
        // public async Task<bool> Put([FromBody]T entity)
        {
            return await _entityRepository.SaveOneAsync(entity);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _entityRepository.RemoveOneAsync(id);
        }

        // public abstract List<BsonDocument> GetNoOfComps();
    }
}
