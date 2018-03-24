﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zulu.Models;

namespace zulu.Data
{
  public class AppDbContext : IdentityDbContext<AppUser>
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Report> Reports { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<EventReport>().HasKey(er => new { er.EventId, er.ReportId });
      modelBuilder.Entity<EventReport>().HasOne(er => er.Event).WithMany(e => e.EventReports);
      modelBuilder.Entity<EventReport>().HasOne(er => er.Report).WithOne(r => r.EventReport);

      modelBuilder.Entity<ContentType>().HasIndex(ct => ct.Name).IsUnique();
      modelBuilder.Entity<Image>().HasOne(i => i.ContentType);

      modelBuilder.Entity<EventImage>().HasKey(ei => new { ei.EventId, ei.ImageId });
      modelBuilder.Entity<EventImage>().HasOne(ei => ei.Event).WithMany(e => e.EventImages);
      // modelBuilder.Entity<EventImage>().HasOne(ei => ei.Image).WithOne(i => i.EventImage);
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
