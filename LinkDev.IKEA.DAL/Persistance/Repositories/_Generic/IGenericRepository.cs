﻿using LinkDev.IKEA.DAL.Entities;
using LinkDev.IKEA.DAL.Entities.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.Repositories._Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync(bool WithAsNoTracking = true);
        public IQueryable<T> GetIQueryable();
        public IEnumerable<T> GetIEnumerable();
        Task<T?> GetAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
