using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulu.Models;

namespace zulu.Controllers
{
  [Route("api/v1")]
  public class ReportController : Controller
  {
    private List<Report> reports;


    public ReportController()
    {
      reports = new List<Report>();

      reports.Add(new Report { Id = 1, Title = "Report One", Content = "Test report for report one." });
      reports.Add(new Report { Id = 2, Title = "Report Two", Content = "Test report for report two." });
      reports.Add(new Report { Id = 3, Title = "Report Three", Content = "Test report for report three." });
      reports.Add(new Report { Id = 4, Title = "Report Four", Content = "Test report for report four." });
    }


    [HttpGet("reports")]
    public IEnumerable<Report> List()
    {
      return reports;
    }


    [HttpPost("report")]
    public IActionResult Post(Report report)
    {
      if (report.Id != 0)
      {
        return BadRequest("Field 'id' not expected.");
      }

      var now = DateTime.Now;

      report.Id = reports.Max(r => r.Id) + 1;  //TODO: don't do this with the database.
      report.Created = now;
      report.LastModified = now;

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
    public IActionResult Put(int id, Report report)
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

      return Ok(report);
    }


    [HttpDelete("report/{id:int}")]
    public IActionResult Delete(int id)
    {
      var report = reports.FirstOrDefault(r => r.Id == id);
      if (report == null)
      {
        return NotFound();
      }

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
