using E_Commerce.eTickets.WebSite.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eTickets.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _appDbContext;
        public EntityBaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _appDbContext.Set<T>().FirstOrDefaultAsync(t => t.Id == id);
            EntityEntry entityEntry = _appDbContext.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll() => await _appDbContext.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] icludePropreties)
        {
            IQueryable<T> query = _appDbContext.Set<T>();
            query = icludePropreties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id) => await _appDbContext.Set<T>().FirstOrDefaultAsync(t => t.Id == id);

        public async Task Update(int id, T entity)
        {
            EntityEntry entityEntry = _appDbContext.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

        }
    }
}
