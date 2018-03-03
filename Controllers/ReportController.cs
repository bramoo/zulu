using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Report;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/reports")]
  [Authorize]
  public class ReportController : Controller
  {
    public ReportController(AppDbContext dbContext, IMapper mapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    private AppDbContext DbContext { get; }
    private IMapper Mapper { get; }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List()
    {
      if (User.Identity.IsAuthenticated)
      {
        return await ListUndeleted();
      }
      else
      {
        return await ListPublished();
      }
    }


    private async Task<IActionResult> ListUndeleted()
    {
      var reports = await DbContext.Reports.Where(r => r.State != Models.EntityState.Deleted).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("draft")]
    public async Task<IActionResult> ListDraft()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Draft).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("published")]
    public async Task<IActionResult> ListPublished()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Published).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("deleted")]
    public async Task<IActionResult> ListDeleted()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Deleted).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("{id:int}", Name = "GetReport")]
    public async Task<IActionResult> Get(int id)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      if (User.Identity.IsAuthenticated || report.State == Models.EntityState.Published)
      {
        return Ok(report);
      }

      return Unauthorized();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      if (report.Delete())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //Should not happen.
    }


    [HttpPost("{id:int}")]
    public async Task<IActionResult> Undelete(int id)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      if (report.UnDelete())
      {
        await DbContext.SaveChangesAsync();
        return CreatedAtAction("GetReport", new { report.Id }, report);
      }

      return BadRequest(); //TODO Error messages.
    }


    [HttpPost("published")]
    public async Task<IActionResult> Publish([FromBody]EditReportViewModel model)
    {
      if (model.Id > 0)
      {
        //Publish existing.
        var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == model.Id);
        if (report == null)
        {
          return NotFound();
        }

        if (report.State == Models.EntityState.Published)
        {
          return NoContent();
        }

        if (report.Publish())
        {
          await DbContext.SaveChangesAsync();
          return NoContent();
        }

      }
      else
      {
        //Create and publish
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var report = Mapper.Map<Report>(model);
        if (report.Publish())
        {
          await DbContext.Reports.AddAsync(report);
          await DbContext.SaveChangesAsync();
          return CreatedAtAction("GetReport", new { report.Id }, Mapper.Map<ReportViewModel>(report));
        }
      }

      return BadRequest(); //TODO Error messages.
    }


    [HttpDelete("published/{id:int}")]
    public async Task<IActionResult> Unpublish(int id)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      if (report.UnPublish())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //TODO Error messages.
    }


    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody]CreateReportViewModel model)
    {
      var report = Mapper.Map<Report>(model);

      await DbContext.Reports.AddAsync(report);
      await DbContext.SaveChangesAsync();
      return CreatedAtAction("GetReport", new { report.Id }, Mapper.Map<ReportViewModel>(report));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]EditReportViewModel model)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      Mapper.Map(model, report);

      await DbContext.SaveChangesAsync();

      return Ok(report);
    }
  }
}
