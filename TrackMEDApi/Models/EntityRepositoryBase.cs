using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace TrackMEDApi
{
    public abstract class EntityRepositoryBase<T> : IEntityRepositoryBase<T> where T : IEntity
    {
        /* Original Code 
        private MongoServer m_Server;
        private MongoDatabase m_Index;

        private MongoCollection<T> m_Entities; 
        */

        private IMongoDatabase m_Index;
        private IMongoCollection<T> m_Entities;

        private readonly IMongoClient m_Client;
        private readonly IMongoDatabase _database;
        private readonly Settings _settings;

        public IMongoCollection<T> Entities
        {
            get
            {
                return m_Entities;
            }
        }

        private string m_Collection;

        /*
        public EntityRepositoryBase(string collection) : this(collection, null, null)
        {

        }
        public EntityRepositoryBase(string collection, string connectionString, string database)
        {
            m_Collection = collection;
            //m_Server = new MongoClient(connectionString).GetServer();
            m_Client = new MongoClient(connectionString);
            m_Index = m_Client.GetDatabase(database);
            m_Entities = m_Index.GetCollection<T>(m_Collection);
        }
        */

        public EntityRepositoryBase(IOptions<Settings> settings, string collection)
        {
            _settings = settings.Value; // reads config.json
            //_database = Connect();
            m_Index = Connect();
            m_Collection = collection;
            m_Entities = m_Index.GetCollection<T>(m_Collection);
        }

        public IEnumerable<T> All()
        {
            return this.m_Entities.AsQueryable<T>().ToList();
        }

        /*
        public IQueryable<T> All(int page, int pageSize)
        {
            //Out of the scope of this article
        }
        */

        public IEnumerable<D> AllAs<D>()
        {
            return m_Entities.AsQueryable<T>().OfType<D>().ToList();
        }

        public T Get(string id)
        {
            //IMongoQuery query = Query.EQ("_id", id);
            var filter = Builders<T>.Filter.Eq("_id", id);
            return this.m_Entities.Find(filter).FirstOrDefault();
        }

        public IQueryable<T> GetFunc(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return this.m_Entities.AsQueryable<T>().Where(expression);
        }

        public IQueryable<T> GetFunc(System.Linq.Expressions.Expression<Func<T, bool>> expression, int page, int pageSize)
        {
            return this.m_Entities.AsQueryable<T>().Where(expression).Skip((page - 1) * pageSize).Take(pageSize);
        }

        /*
        public IQueryable<T> GetAs<D>(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return m_Entities.FindAllAs<D>().Cast<T>().ToList().AsQueryable().Where(expression);
        } */

        public virtual T Add(T entity)
        {
            try
            {
                IEntity oEntity = (entity as IEntity);

                /*
                oEntity.Id = String.IsNullOrEmpty(oEntity.Id) ?
                    ObjectId.GenerateNewId().ToString() :
                    oEntity.Id;

                m_Entities.Insert(entity);
                */

                //oEntity.Id = oEntity.Id == null ?
                //            ObjectId.GenerateNewId() :
                //            oEntity.Id;

                m_Entities.InsertOne(entity);

                return entity;
            }
            catch (Exception mongoException)
            {
                if (mongoException.HResult == -2146233088)
                {
                    //throw new MongoEntityUniqueIndexException("Unique Index violation", mongoException);
                }
                else
                {
                    throw mongoException;
                }
            }

            return default(T);
        }

        public virtual int Add(IEnumerable<T> entities)
        {
            int addCount = 0;

            entities.ToList().ForEach(entity =>
            {
                if (Add(entity) != null)
                {
                    addCount++;
                }
            });

            return addCount;
        }

        public virtual void AddBatch(IEnumerable<T> entities)
        {
            //int addCount = 0;

            entities.ToList().ForEach(entity =>
            {
                IEntity oEntity = (entity as IEntity);

                /*
                oEntity.Id = String.IsNullOrEmpty(oEntity.Id) ?
                    ObjectId.GenerateNewId().ToString() :
                    oEntity.Id;

                oEntity.Created = timeStamp;
                oEntity.LastModified = timeStamp;
                */

                //oEntity.Id = oEntity.Id == null ?
                //    ObjectId.GenerateNewId() :
                //    oEntity.Id;
                
            });

            try
            {
                //m_Entities.InsertBatch(entities);
                m_Entities.InsertMany(entities);
            }
            catch 
            {
                return;
            }
        }

        public virtual void Remove(T entity)
        {
            //Remove(entity.Id);
        }

        public virtual bool Remove(string id)
        {
            try
            {
                /*
                IMongoQuery query = Query.EQ("_id", id);
                var result = m_Entities.Remove(filter);
                return result.DocumentsAffected == 1;
                */

                var filter = Builders<T>.Filter.Eq("_id", id);
                
                var result = m_Entities.DeleteOne(filter);
                return result.DeletedCount == 1;
                
            }
            catch
            {
                //return false;
            }

            return false;
        }

        /*
        Remove All Documents (See https://docs.mongodb.com/getting-started/csharp/remove/)

        To remove all documents from a collection, pass an empty conditions document to the DeleteManyAsync method.

        var collection = _database.GetCollection<BsonDocument>("restaurants");
        var filter = new BsonDocument();
        var result = await collection.DeleteManyAsync(filter);
         */
        public virtual bool RemoveAll()
        {
            try
            {
                //var result = m_Entities.RemoveAll();
                //return result.DocumentsAffected == 1;

                var filter = new BsonDocument();
                var result = m_Entities.DeleteManyAsync(filter);
                
                return result.IsCompleted;
            }
            catch 
            {
                
            }

            return false;
        }

        /*
        public virtual int Remove(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            int removeCount = 0;
            List<T> entitiesToRemove = this.m_Entities.AsQueryable<T>().Where(expression).ToList();

            entitiesToRemove.ForEach(entity =>
            {
                if (Remove((entity as IEntity).Id))
                {
                    removeCount++;
                }
            });

            return removeCount;
        } */

        public virtual T Update(T updatedEntity)
        {
            return Update(updatedEntity);
        }

        private IMongoDatabase Connect()
        {
            var client = new MongoClient(_settings.MongoConnection);
            var database = client.GetDatabase(_settings.Database);

            return database;
        }

    }
}
