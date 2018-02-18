using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/reports")]
  public class ReportController : EntityController<Report>
  {
    //private static List<Report> reports;

    //static ReportController()
    //{
    //  reports = new List<Report>
    //  {
    //    new Report { Id = 1, Title = "Report One", Content = "Test report for report one." },
    //    new Report { Id = 2, Title = "Report Two", Content = "Test report for report two." },
    //    new Report { Id = 3, Title = "Report Three", Content = "Test report for report three." },
    //    new Report { Id = 4, Title = "Report Four", Content = "Test report for report four." }
    //  };
    //}

    public ReportController(AppDbContext dbContext)
      : base(dbContext)
    {
    }


    protected override DbSet<Report> Entities => DbContext.Reports;


    [HttpGet("", Name = "AllReports")]
    public new async Task<IActionResult> List() => await base.List();


    [HttpGet("draft", Name = "DraftReports")]
    public new async Task<IActionResult> ListDraft() => await base.ListDraft();


    [HttpGet("published", Name = "PublishedReports")]
    public new async Task<IActionResult> ListPublished() => await base.ListPublished();


    [HttpGet("draft", Name = "DeletedReports")]
    public new async Task<IActionResult> ListDeleted() => await base.ListDeleted();


    [HttpGet("{id:int}", Name = "GetReports")]
    public new async Task<IActionResult> Get(int id) => await base.Get(id);


    [HttpDelete("{id:int}")]
    public new async Task<IActionResult> Delete(int id) => await base.Delete(id);


    [HttpPost("", Name = "UndeleteReports")]
    public new async Task<IActionResult> Undelete([FromBody]int id, [FromBody]Report model) => await base.Undelete(id, model);


    [HttpPost("published")]
    public new async Task<IActionResult> Publish([FromBody]int id) => await base.Publish(id);


    [HttpDelete("published/{id:int}")]
    public new async Task<IActionResult> Unpublish(int id) => await base.Unpublish(id);




    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]Report model)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(r => r.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      var now = DateTime.Now;

      report.Title = model.Title;
      report.Content = model.Content;
      report.Author = model.Author;
      report.Modified = now;

      await DbContext.SaveChangesAsync();

      return Ok(report);
    }

  }
}
