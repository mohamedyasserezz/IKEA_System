﻿using LinkDev.IKEA.DAL.Entities;
using LinkDev.IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.Repositories._Generic
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public IEnumerable<T> GetAll(bool WithAsNoTracking = true)
        {
            if (WithAsNoTracking)
                return _dbContext.Set<T>().AsNoTracking().ToList();
            return _dbContext.Set<T>().ToList();
        }
        public IQueryable<T> GetAllAsIQueryable()
        {
            return _dbContext.Set<T>();
        }
        public T? Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
            //return _dbContext.Find<T>(id);

            /// var T = _dbContext.Set<T>().Local.FirstOrDefault(d => d.Id == id);
            /// if (T == null)
            ///     T = _dbContext.Set<T>().FirstOrDefault(d => d.Id == id);
            ///     return T;

        }
        public int Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges();
        }
        public int Update(T entity)
        {
            _dbContext.Update(entity);
            return _dbContext.SaveChanges();
        }
        public int Delete(T entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }
    }
}
