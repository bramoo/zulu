﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulu.Models;

namespace zulu.Data
{
  public class SeedData
  {
    internal static void Initialize(IServiceProvider services)
    {
      var dbContext = (AppDbContext)services.GetService(typeof(AppDbContext));

      if (!dbContext.Reports.Any())
      {
        dbContext.Reports.Add(new Report { Title = "Report One", Content = "Test report for report one." });
        dbContext.Reports.Add(new Report { Title = "Report Two", Content = "Test report for report two." });
        dbContext.Reports.Add(new Report { Title = "Report Three", Content = "Test report for report three." });
        dbContext.Reports.Add(new Report { Title = "Report Four", Content = "Test report for report four." });
      }


      if (!dbContext.Events.Any())
      {
        dbContext.Events.Add(new Event { Name = "Event 1", Start = DateTime.Today, End = DateTime.Today.AddDays(1), AllDay = true });
        dbContext.Events.Add(new Event { Name = "Event 2", Start = DateTime.Today.AddHours(9), End = DateTime.Today.AddHours(12), AllDay = false });
        dbContext.Events.Add(new Event { Name = "Event 3", Start = DateTime.Today, End = DateTime.Today.AddDays(2), AllDay = true });
      }


      dbContext.SaveChanges();
    }
  }
}
