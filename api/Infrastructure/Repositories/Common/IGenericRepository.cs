using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlmacenSystem.Infrastructure.Repositories.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(List<string> includes = null);
        Task<T> GetByIdAsync(object id, List<string> includes = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task<int> SaveChangesAsync();
        void SaveChanges();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, List<string> includes = null);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, List<string> includes = null);
    }
}