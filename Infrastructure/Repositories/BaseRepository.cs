using Application.Contracts.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<T>(OMIIasiDbContext context) : IAsyncRepository<T>
    where T : class
{
    public virtual async Task<Result<T>> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> DeleteAsync(Guid id)
    {
        var result = await FindByIdAsync(id);

        if (!result.IsSuccess)
            return Result<T>.Failure($"{nameof(T)} with id {id} not found");

        context.Set<T>().Remove(result.Value);
        await context.SaveChangesAsync();
        return Result<T>.Success(result.Value);
    }

    public virtual async Task<Result<T>> FindByIdAsync(Guid id)
    {
        var result = await context.Set<T>().FindAsync(id);
        return result == null ? Result<T>.Failure($"{nameof(T)} with id {id} not found") : Result<T>.Success(result);
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetPagedResponseAsync(int page, int size)
    {
        var result = await context.Set<T>().Skip(page).Take(size).AsNoTracking().ToListAsync();
        return Result<IReadOnlyList<T>>.Success(result);
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetAllAsync()
    {
        var result = await context.Set<T>().AsNoTracking().ToListAsync();
        return Result<IReadOnlyList<T>>.Success(result);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id) != null;
    }

    public virtual async Task<Result<T>> UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Result<T>.Success(entity);
    }
}