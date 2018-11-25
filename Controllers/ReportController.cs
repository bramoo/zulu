using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;
using zulu.ViewModels.Mapper;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/reports")]
  [Authorize]
  public class ReportController : Controller
  {
    public ReportController(AppDbContext dbContext, ReportMapper mapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    private AppDbContext DbContext { get; }
    private ReportMapper Mapper { get; }


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
      var reports = await DbContext.Reports.Where(r => r.State != Models.EntityState.Deleted).Select(r => Mapper.Map(r)).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("draft")]
    public async Task<IActionResult> ListDraft()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Draft).Select(r => Mapper.Map(r)).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("published")]
    public async Task<IActionResult> ListPublished()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Published).Select(r => Mapper.Map(r)).ToListAsync();
      return Ok(reports);
    }


    [HttpGet("deleted")]
    public async Task<IActionResult> ListDeleted()
    {
      var reports = await DbContext.Reports.Where(r => r.State == Models.EntityState.Deleted).Select(r => Mapper.Map(r)).ToListAsync();
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
        return Ok(Mapper.Map(report));
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


    //[HttpPost("{id:int}")]
    //public async Task<IActionResult> Undelete(int id)
    //{
    //  var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
    //  if (report == null)
    //  {
    //    return NotFound();
    //  }

    //  if (report.UnDelete())
    //  {
    //    await DbContext.SaveChangesAsync();
    //    return CreatedAtRoute("GetReport", new { report.Id }, report);
    //  }

    //  return BadRequest(); //TODO Error messages.
    //}


    [HttpPost("published")]
    public async Task<IActionResult> Publish([FromBody]ReportViewModel model)
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

        var report = Mapper.Update(new Report(), model);
        if (report.Publish())
        {
          await DbContext.Reports.AddAsync(report);
          await DbContext.SaveChangesAsync();
          return CreatedAtRoute("GetReport", new { report.Id }, Mapper.Map(report));
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
    public async Task<IActionResult> Post([FromBody]ReportViewModel model)
    {
      var report = Mapper.Update(new Report(), model);

      await DbContext.Reports.AddAsync(report);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetReport", new { report.Id }, Mapper.Map(report));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]ReportViewModel model)
    {
      var report = await DbContext.Reports.SingleOrDefaultAsync(e => e.Id == id);
      if (report == null)
      {
        return NotFound();
      }

      Mapper.Update(report, model);

      await DbContext.SaveChangesAsync();

      return Ok(Mapper.Map(report));
    }
  }
}
