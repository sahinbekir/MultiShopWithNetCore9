using System.Linq.Expressions;

namespace MultiShopWithNetCore9.Order.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter);
}
