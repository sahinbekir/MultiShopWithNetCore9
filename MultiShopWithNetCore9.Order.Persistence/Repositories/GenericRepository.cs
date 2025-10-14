using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Persistence.Context;

namespace MultiShopWithNetCore9.Order.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly OrderContext _context;

    public GenericRepository(OrderContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(filter);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
