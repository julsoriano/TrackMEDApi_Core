﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrackMEDApi.Models;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;
using MongoDB.Bson;

namespace TrackMEDApi
{
    public interface IEntityRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetSelectedAsync(string tableID, string Id);
        //Task<IEnumerable<T>> GetManyAsync(string[] ids);
        Task<T> GetOneAsync(string id);
        Task<T> GetOneAsyncByDescription(string Description);
        Task<T> GetOneAsyncByFieldID(string fieldID, string Id);
        Task<bool> AddOneAsync(T entity);
        Task<bool> SaveOneAsync(T entity);
        Task<bool> RemoveOneAsync(string id);
        void DropDatabase();
        /*

        Task<T> GetOneAsync(T context);
        
        Task<T> GetManyAsync(IEnumerable<T> contexts);
        Task<T> GetManyAsync(IEnumerable<string> ids);
        Task<T> SaveOneAsync(T Context);
        Task<T> SaveManyAsync(IEnumerable<T> contexts);
        Task<bool> RemoveOneAsync(T context);
        Task<bool> RemoveOneAsync(string id);
        Task<bool> RemoveManyAsync(IEnumerable<T> contexts);
        Task<bool> RemoveManyAsync(IEnumerable<string> ids);
        */

        /* Original
        //IEnumerable<T> All();
        //IQueryable<T> All(int page, int pageSize);
        //T Get(string id);
        //IQueryable<T> GetFunc(Expression<Func<T, bool>> expression);
        //T Add(T entity);
        //int Add(IEnumerable<T> entities);
        //void Remove(T entity);
        //bool Remove(string id);
        //bool RemoveAll();
        //int Remove(Expression<Func<T, bool>> expression);
        //T Update(T updatedEntity);
        */
    }
}
