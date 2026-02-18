using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Identity;

namespace MiniSkeletonAPI.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<DataCounting> DataCounts => Set<DataCounting>();
    public DbSet<DataAndonDetail> DataAndonDetails => Set<DataAndonDetail>();
    public DbSet<MRepair> MRepairs => Set<MRepair>();
    public DbSet<DataAndon> DataAndons => Set<DataAndon>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<MWarna> MWarnas => Set<MWarna>();
    public DbSet<ApplicationRole> Roles => Set<ApplicationRole>();


    public DbSet<ApplicationUserRole> UserRoles => Set<ApplicationUserRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId);
        });
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    internal Task<T> ExecuteScalarAsync<T>(string totalSql)
    {
        throw new NotImplementedException();
    }
}
