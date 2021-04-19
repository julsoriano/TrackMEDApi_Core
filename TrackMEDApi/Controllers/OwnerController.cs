using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMEDApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackMEDApi.Controllers
{
    [Route("api/[controller]")]
    public class OwnerController : ApiControllerCommon<Owner>
    {
        public readonly IMongoClient m_Client;
        public readonly IMongoDatabase _database;
        public IMongoCollection<Owner> m_Entities;
        private readonly Settings _settings;
        public readonly IMongoDatabase _db;

        // public OwnerController(IEntityRepository<Owner> entityService, EntityRepository<Owner> entity) 
        public OwnerController(IEntityRepository<Owner> entityService, IOptions<Settings> optionsAccessor) 
                : base(entityService)
        {
            _settings = optionsAccessor.Value; // reads appsettings.json

            // http://www.codeproject.com/Articles/1077319/Csharp-MongoDB-Polymorphic-Collections-with-Generi
            m_Client = new MongoClient(_settings.MongoConnection);
            _database = m_Client.GetDatabase(_settings.Database);

            //_database = Connect();
            m_Entities = _database.GetCollection<Owner>(typeof(Owner).Name);
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
        
        [HttpGet]
        [Route("GetNoOfComps")]
        public List<OwnerComponent> GetNoOfComps()
        {
            // Begin: Cloned from https://www.niceonecode.com/question/20675/mongodb-join-string-id-with-objectid-in-aggregate-lookup
            BsonDocument expression = new BsonDocument {
                new BsonElement("ownerStringId", new BsonDocument(new BsonElement("$toString", "$_id")))
                    };
            BsonDocument addFieldsStage = new BsonDocument(new BsonElement("$addFields", expression));

            // m_Entities is injected thru IOptions (TODO: Find a way to access the field in EntityRepository class)
            var aggregate = m_Entities.Aggregate()
                           .Sort(new BsonDocument { { "Desc", 1 } })
                           .AppendStage<BsonDocument>(addFieldsStage)
                           .Lookup("Component", "ownerStringId", "OwnerID", @as: "owner_docs")
                           .Project(new BsonDocument { { "Desc", 1 }, { "CreatedAtUtc", 1 }, { "NoOfComponents", new BsonDocument { { "$size", "$owner_docs" } } } })
                           .ToList(); 
            // End
            
            List<OwnerComponent> oc = new List<OwnerComponent>();
            for(int i = 0; i < aggregate.Count; i++)
            {
                // See https://stackoverflow.com/questions/24375928/how-to-convert-a-bsondocument-into-a-strongly-typed-object-with-the-official-mon
                var obj = BsonSerializer.Deserialize<OwnerComponent>(aggregate[i]);
                oc.Add(obj);
            }
            /* Does Not Work https://stackoverflow.com/questions/27132968/convert-mongodb-bsondocument-to-valid-json-in-c-sharp
            var dotNetObj = BsonTypeMapper.MapToDotNetValue(aggregate[0]);
            var dotNetObjList = aggregate.ConvertAll(BsonTypeMapper.MapToDotNetValue);
            */
            return oc;
        }      
    }
}
