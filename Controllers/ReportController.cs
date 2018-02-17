using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1")]
  public class ReportController : Controller
  {
    private static List<Report> reports;

    static ReportController()
    {
      reports = new List<Report>
      {
        new Report { Id = 1, Title = "Report One", Content = "Test report for report one." },
        new Report { Id = 2, Title = "Report Two", Content = "Test report for report two." },
        new Report { Id = 3, Title = "Report Three", Content = "Test report for report three." },
        new Report { Id = 4, Title = "Report Four", Content = "Test report for report four." }
      };
    }

    public ReportController()
    {
    }


    [HttpGet("reports")]
    public IActionResult List(int? page, int? pageSize, bool? deleted)
    {
      //page = page ?? 1;
      //pageSize = pageSize ?? 100;

      //return Ok(new Page<Report>(reports.Where(r => r.Deleted == null).Skip(, null, null));

      return Ok(reports.Where(r => r.Deleted == null));
    }


    [HttpGet("reports/published")]
    public IActionResult ListPublished()
    {
      return Ok(reports.Where(r => r.Published != null));
    }


    [HttpPost("report")]
    public IActionResult Post([FromBody]Report report)
    {
      if (report.Id != 0)
      {
        return BadRequest("Field 'id' not expected.");
      }

      var now = DateTime.Now;

      report.Id = reports.Max(r => r.Id) + 1;  //TODO: don't do this with the database.
      report.Created = now;
      report.LastModified = now;

      reports.Add(report);

      return Created(new Uri($"/report/{report.Id}", UriKind.Relative), report);
    }


    [HttpGet("report/{id:int}")]
    public IActionResult Get(int id)
    {
      if (!reports.Any(r => r.Id == id))
      {
        return NotFound();
      }

      return Ok(reports.First(r => r.Id == id));
    }


    [HttpPut("report/{id:int}")]
    public IActionResult Put(int id, [FromBody]Report report)
    {
      var orig = reports.FirstOrDefault(r => r.Id == id);
      if (orig == null)
      {
        return NotFound();
      }

      var now = DateTime.Now;

      orig.Title = report.Title;
      orig.Content = report.Content;
      orig.Author = report.Author;
      orig.LastModified = now;

      return Ok(orig);
    }


    [HttpDelete("report/{id:int}")]
    public IActionResult Delete(int id)
    {
      var report = reports.FirstOrDefault(r => r.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      report.Published = null;
      report.Deleted = DateTime.Now;

      return NoContent();
    }


    [HttpPost("report/{id:int}/publish")]
    public IActionResult Publish(int id)
    {
      var report = reports.FirstOrDefault(r => r.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      report.Published = DateTime.Now;

      return Ok(report);
    }
  }
}
