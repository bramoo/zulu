using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zulu.Models;

namespace zulu.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<Event> Events { get; set; }
    public DbSet<Report> Reports { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<EventReport>().HasKey(er => new { er.EventId, er.ReportId });
      modelBuilder.Entity<Event>().HasMany(e => e.Reports).WithOne(r => r.Event);
    }

    #region SaveChanges Overrides

    public override int SaveChanges()
    {
      UpdateTimeStamps();
      return base.SaveChanges();
    }


    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
      UpdateTimeStamps();
      return base.SaveChanges(acceptAllChangesOnSuccess);
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
      UpdateTimeStamps();
      return base.SaveChangesAsync(cancellationToken);
    }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
      UpdateTimeStamps();
      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    #endregion SaveChanges Overrides

    #region Private Methods

    private void UpdateTimeStamps()
    {
      var entities = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == Microsoft.EntityFrameworkCore.EntityState.Added || x.State == Microsoft.EntityFrameworkCore.EntityState.Modified));

      var now = DateTime.Now;

      foreach (var item in entities)
      {
        var entity = ((Entity)item.Entity);
        if (item.State == Microsoft.EntityFrameworkCore.EntityState.Added)
        {
          entity.Created = now;
        }

        entity.Modified = now;
      }
    }

    #endregion Private Methods
  }
}
