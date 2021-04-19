using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackMEDApi
{
    public class EntityRepository<T>: IEntityRepository<T> where T : IEntity
    {
        public readonly IMongoClient m_Client;
        public readonly IMongoDatabase _database;
        public IMongoCollection<T> m_Entities;
        private readonly Settings _settings;

        /*
        For the use of IOptions<Settings> to access configuration settings:
            See Configuration Using Options and Configuration Objects 
                https://docs.asp.net/en/latest/fundamentals/configuration.html#options-config-objects
            Or Strongly-Typed Configuration Settings in ASP.NET Code 
                https://weblog.west-wind.com/posts/2016/may/23/strongly-typed-configuration-settings-in-aspnet-core#HookinguptheConfiguration
        */
        public EntityRepository(IOptions<Settings> optionsAccessor)
        {
            _settings = optionsAccessor.Value; // reads appsettings.json
            
            // http://www.codeproject.com/Articles/1077319/Csharp-MongoDB-Polymorphic-Collections-with-Generi
            m_Client = new MongoClient(_settings.MongoConnection);
            _database = m_Client.GetDatabase(_settings.Database);
            
            //_database = Connect();
            m_Entities = _database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await m_Entities.Find(new BsonDocument()).ToListAsync(); 
        }
       
        /*
        public async Task<IEnumerable<T>> GetManyAsync(string[] ids)
        {
            List<T> components = new List<T>(); 
            foreach(string id in ids) {
                var doc = await GetOneAsync(id);
                if (doc == null) continue;
                components.Add(doc);
            };
            return components;
        }
        */

        public async Task<T> GetOneAsync(string id)
        {
            var query = Builders<T>.Filter.Eq("Id", id);
            return await m_Entities.Find(query).FirstOrDefaultAsync();       // or: return await m_Entities.Find(new BsonDocument("_id", id)).FirstOrDefaultAsync();
        }

        public async Task<T> GetOneAsyncByDescription(string Description)
        {
            var query = Builders<T>.Filter.Eq("Desc", " ");
            if (typeof(T).Name == "SystemTab" || typeof(T).Name == "Component" || typeof(T).Name == "Event")
            {
                query = Builders<T>.Filter.Eq("imte", Description.Trim());
            }
            else
                if(typeof(T).Name == "Deployment")
                {
                    query = Builders<T>.Filter.Eq("SystemTabID", Description.Trim());
                }
                else       
                {
                // http://stackoverflow.com/questions/1405048/how-do-i-decode-a-url-parameter-using-c
                /*
                    private static string DecodeUrlString(string url) {
                        string newUrl;
                        while ((newUrl = Uri.UnescapeDataString(url)) != url)
                            url = newUrl;
                        return newUrl;
                    }             
                */

                Description = Uri.UnescapeDataString(Description);
                query = Builders<T>.Filter.Eq("Desc", Description.Trim());
                }

            /*
            var query = Builders<T>.Filter.Eq(fieldID, Id.Trim());
            return await m_Entities.Find(query).FirstOrDefaultAsync();
            */

            return await m_Entities.Find(query).FirstOrDefaultAsync();
        }

        public async Task<T> GetOneAsyncByFieldID(string fieldID, string Id)
        {
            var query = Builders<T>.Filter.Eq(fieldID, Id.Trim());
            return await m_Entities.Find(query).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetSelectedAsync(string tableID, string Id)
        {
            var query = Builders<T>.Filter.Eq("DescriptionID", Id.Trim());
            switch (tableID)
            {
                case "Deployment":
                    query = Builders<T>.Filter.Eq("SystemTabID", Id.Trim());
                    break;

                case "Description":
                    query = Builders<T>.Filter.Eq("DescriptionID", Id.Trim());
                    break;

                case "EquipmentActivity":
                    query = Builders<T>.Filter.Eq("DeploymentID", Id.Trim());
                    break;

                case "EquipmentActivityID":
                    query = Builders<T>.Filter.Eq("imte", Id.Trim());
                    break;

                case "Location":
                    query = Builders<T>.Filter.Eq("LocationID", Id.Trim());
                    break;

                case "Model_Manufacturer":
                    query = Builders<T>.Filter.Eq("Model_ManufacturerID", Id.Trim());
                    break;

                case "Owner":
                    query = Builders<T>.Filter.Eq("OwnerID", Id.Trim());
                    break;

                case "ProviderOfService":
                    query = Builders<T>.Filter.Eq("ProviderOfServiceID", Id.Trim());
                    break;

                case "Status":
                    query = Builders<T>.Filter.Eq("StatusID", Id.Trim());
                    break;

                default:
                    break;
            }

            return await m_Entities.Find(query).ToListAsync();
        }

        public async Task<bool> AddOneAsync(T entity)
        {
            await m_Entities.InsertOneAsync(entity);
            return GetOneAsync(entity.Id) != null;
        }

        public async Task<ReplaceOneResult> SaveOneAsync(T entity)
        // public async Task<bool> SaveOneAsync(T entity)
        {
            var query = Builders<T>.Filter.Eq(e => e.Id, entity.Id); // var query = Builders<T>.Filter.Eq("Id", entity.Id);

            /*
             To replace the entire document except for the _id field, pass an entirely new document as the second argument to the ReplaceOneAsync method. 
             The replacement document can have different fields from the original document. In the replacement document, you can omit the _id field since the _id field is immutable. 
             If you do include the _id field, it must be the same value as the existing value.             
             See: https://docs.mongodb.com/getting-started/csharp/update/
             */
            return await m_Entities.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = true });
            // ReplaceOneResult result = await m_Entities.ReplaceOneAsync(query, entity);
            // return result.ModifiedCount > 0;
        }

        public async Task<bool> RemoveOneAsync(string id)
        {
            var query = Builders<T>.Filter.Eq("Id", id);
            var result = await m_Entities.DeleteOneAsync(query);
            return GetOneAsync(id) == null;  // or: return result.IsAcknowledged;       
        }

        public void DropDatabase()
        {
            m_Client.DropDatabaseAsync(_settings.Database).Wait();
        }

        #region Private Helper Methods
        /*
        private IMongoDatabase Connect()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("trackmed");

            return database;
        }
        */
        #endregion
    }
}
