using System.Linq.Expressions;

namespace MiniSkeletonAPI.Application.Common.Models;

public class SingleResult<T>
{
    public T? Data { get; }
    public bool IsFound => Data != null;

    public SingleResult(T? data)
    {
        Data = data;
    }

    public static async Task<SingleResult<T>> CreateAsync(
        IQueryable<T> source,
        Expression<Func<T, bool>> predicate)
    {
        var data = await source.FirstOrDefaultAsync(predicate);
        return new SingleResult<T>(data);
    }
}
