using MiniSkeletonAPI.Domain.Entities;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<DataCounting> DataCounts { get; }
    DbSet<MRepair> MRepairs { get; }
    DbSet<DataAndon> DataAndons { get; }
    DbSet<Setting> Settings { get; }
    DbSet<Part> Parts { get; }
    DbSet<MWarna> MWarnas { get; }
    DbSet<DataAndonDetail> DataAndonDetails { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
