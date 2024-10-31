using Microsoft.EntityFrameworkCore;
using ProductCatalog.Repository.Common.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Common.BaseRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbContext Entities;
        protected readonly DbSet<T> DbSet;
        public GenericRepository(IDbContext context)
        {
            Entities = context;
            DbSet = context.Set<T>();
        }
        public virtual IQueryable<T> GetALl()
        {
            return DbSet;
        }
        public virtual async Task<T?> GetByIdAsync<TDataType>(TDataType id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
            {
                return default;
            }

            switch (id)
            {
                case int:
                    {
                        var a = Convert.ToInt32(id);
                        if (a <= 0)
                        {
                            return default;
                        }

                        break;
                    }
                case Guid when Guid.Parse(id.ToString() ?? string.Empty) == Guid.Empty:
                    return default;
            }

            return await DbSet.FindAsync(id);
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            return (await DbSet.AddAsync(entity)).Entity;
        }

        public virtual T Delete(T entity)
        {
            DbSet.Attach(entity);
            return DbSet.Remove(entity).Entity;
        }



        public virtual T UpdateAsync(T entity, params string[] properties)
        {
            DbSet.Attach(entity);
            if (properties.Length == 0)
            {
                Entities.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                foreach (var property in properties)
                {
                    Entities.Entry(entity).Property(property).IsModified = true;
                }
            }
            return entity;
        }




        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }


        public virtual T Edit(T entity, string parameter)
        {
            DbSet.Attach(entity);
            Entities.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual async Task<T> DeleteByIdAsync<TDataType>(TDataType id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == default(T) || entity is null)
            {
                return default;
            }
            return Delete(entity);
        }


    }
}
