using System;
using System.Linq;
using zulu.Models;

namespace zulu.Data
{
  public class SeedData
  {
    internal static void Initialize(IServiceProvider services)
    {
      var dbContext = (AppDbContext)services.GetService(typeof(AppDbContext));

      if (!dbContext.ContentTypes.Any())
      {
        dbContext.ContentTypes.Add(new ContentType { Name = "image/jpeg" });
        dbContext.ContentTypes.Add(new ContentType { Name = "image/png" });
        dbContext.ContentTypes.Add(new ContentType { Name = "image/gif" });

        //TODO: should probably add more content types.
      }


      if (!dbContext.MemberRanks.Any())
      {
        dbContext.MemberRanks.Add(new MemberRank { Rank = "Squire" });
        dbContext.MemberRanks.Add(new MemberRank { Rank = "Knight" });
        dbContext.MemberRanks.Add(new MemberRank { Rank = "Associate" });
        dbContext.MemberRanks.Add(new MemberRank { Rank = "Life" });
      }


      if (!dbContext.MemberRanks.Any())
      {
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Crew Leader" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Assistant Crew Leader" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Treasurer" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Secretary" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Youth Liaison" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Trading Manager" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Master of Ceremonies" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Mascot" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Trapline Manager" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Quatermaster" });
        dbContext.MemberPositions.Add(new MemberPosition { Name = "Sheriff" });
      }


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
