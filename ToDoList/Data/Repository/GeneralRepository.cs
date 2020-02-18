using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Base;
using Data.Context;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class GeneralRepository <TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly MyContext _myContext;

        public GeneralRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await _myContext.Set<TEntity>().FindAsync(id);
            if(entity == null)
            {
                return entity;
            }
            _myContext.Set<TEntity>().Remove(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _myContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await _myContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Post(TEntity entity)
        {
            await _myContext.Set<TEntity>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Put(TEntity entity)
        {
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }
    }
}
