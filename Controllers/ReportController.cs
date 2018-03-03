using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
	[Produces("application/json")]
  [Route("api/v1/reports")]
  public class ReportController : EntityController<Report>
  {
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


    [HttpGet("deleted", Name = "DeletedReports")]
    public new async Task<IActionResult> ListDeleted() => await base.ListDeleted();


    [HttpGet("{id:int}", Name = "GetReports")]
    public new async Task<IActionResult> Get(int id) => await base.Get(id);


    [HttpDelete("{id:int}")]
    public new async Task<IActionResult> Delete(int id) => await base.Delete(id);


    [HttpPost("{id:int}", Name = "UndeleteReports")]
    public new async Task<IActionResult> Undelete(int id) => await base.Undelete(id);


    [HttpPost("published")]
    public new async Task<IActionResult> Publish([FromBody]Report model) => await base.Publish(model);


    [HttpDelete("published/{id:int}")]
    public new async Task<IActionResult> Unpublish(int id) => await base.Unpublish(id);


    [HttpDelete("published")]
    public async Task<IActionResult> UnpublishFB([FromBody]Report model) => await base.Unpublish(model.Id);


    [HttpPost("")]
    public new async Task<IActionResult> Post([FromBody]Report model) => await base.Post(model);


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
