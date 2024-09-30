using LinkDev.IKEA.DAL.Entities;
using LinkDev.IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.Repositories._Generic
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<IEnumerable<T>> GetAllAsync(bool WithAsNoTracking = true)
        {
            if (WithAsNoTracking)
                return await _dbContext.Set<T>().Where(E => !E.IsDeleted).AsNoTracking().ToListAsync();
            return await _dbContext.Set<T>().Where(E => !E.IsDeleted).ToListAsync();
        }
        public IQueryable<T> GetIQueryable()
        {
            return _dbContext.Set<T>().Where(E => !E.IsDeleted);
        }
        public IEnumerable<T> GetIEnumerable()
        {
            throw new NotImplementedException();
        }
        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
            //return _dbContext.Find<T>(id);

            /// var T = _dbContext.Set<T>().Local.FirstOrDefault(d => d.Id == id);
            /// if (T == null)
            ///     T = _dbContext.Set<T>().FirstOrDefault(d => d.Id == id);
            ///     return T;

        }
        public void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public void Update(T entity) => _dbContext.Update(entity);

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Update(entity);
        }

       
    }
}
