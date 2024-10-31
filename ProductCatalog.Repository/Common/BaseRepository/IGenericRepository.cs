using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Common.BaseRepository
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetALl();
        Task<T?> GetByIdAsync<TDataType>(TDataType id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);

        T Edit(T entity, string parameter);
        T UpdateAsync(T entity, params string[] properties);
        T Delete(T entity);
        Task<T> DeleteByIdAsync<TDataType>(TDataType id);
    }
}
