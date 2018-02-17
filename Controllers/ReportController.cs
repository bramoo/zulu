using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/reports")]
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


    [HttpGet("")]
    public IActionResult List(bool deleted = false)
    {
      var query = (IEnumerable<Report>)reports;
      if(!deleted)
      {
        query = query.Where(r => r.Deleted == null);
      }

      var results = reports.Select(r => new ReportListViewModel
      {
        Id = r.Id,
        Title = r.Title,
        Author = r.Author,
        Published = r.Published.HasValue,
        Deleted = r.Deleted.HasValue
      });

      return Ok(results);
    }
    
    
    [HttpPost("")]
    public IActionResult Post([FromBody]ReportPostViewModel model)
    {
      var now = DateTime.Now;

      var report = new Report
      {
        Id = reports.Max(r => r.Id) + 1, //TODO: don't do this with the database.
        Title = model.Title,
        Content = model.Content,
        Author = model.Author,
        Created = now,
        LastModified = now,
      };

      reports.Add(report);

      return Created(Url.Link("GetReport", new { report.Id }), report);
    }


    [HttpGet("{id:int}", Name = "GetReport")]
    public IActionResult Get(int id)
    {
      if (!reports.Any(r => r.Id == id))
      {
        return NotFound();
      }

      var report = reports.First(r => r.Id == id);
      var model = new ReportGetViewModel
      {
        Id = report.Id,
        Title = report.Title,
        Author = report.Author,
        Content = report.Content,
        Created = report.Created,
        LastModified = report.LastModified,
        Deleted = report.Deleted.HasValue,
        Published = report.Published.HasValue
      };

      return Ok(model);
    }


    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody]ReportPostViewModel model)
    {
      var report = reports.FirstOrDefault(r => r.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      var now = DateTime.Now;

      report.Title = model.Title;
      report.Content = model.Content;
      report.Author = model.Author;
      report.LastModified = now;

      return Ok(report);
    }


    [HttpDelete("{id:int}")]
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


    [HttpPost("{id:int}/published")]
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


    [HttpGet("published")]
    public IActionResult ListPublished(int offset = 0, int limit = 20)
    {
      var query = reports.Where(r => r.Deleted == null && r.Published != null);

      var results = reports.Select(r => new ReportListViewModel
      {
        Id = r.Id,
        Title = r.Title,
        Author = r.Author,
        Published = r.Published.HasValue,
        Deleted = r.Deleted.HasValue
      });

      return Ok(results);
    }


    [HttpPost("published")]
    [HttpPut("published")]
    public IActionResult Publish([FromBody]ReportPublishViewModel model)
    {
      var report = reports.FirstOrDefault(r => r.Id == model.Id);
      if (report == null)
      {
        return BadRequest();
      }

      report.Published = DateTime.Now;

      return Ok(report);
    }
  }
}
