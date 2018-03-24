using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using zulu.Attributes;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Event;
using zulu.ViewModels.Image;
using zulu.ViewModels.Report;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/events")]
  [Authorize]
  public class EventController : Controller
  {
    public EventController(AppDbContext dbContext, IMapper mapper)
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
      var events = await DbContext.Events.Where(e => e.State != Models.EntityState.Deleted).Select(e => Mapper.Map<EventViewModel>(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("draft")]
    public async Task<IActionResult> ListDraft()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Draft).Select(e => Mapper.Map<EventViewModel>(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("published")]
    public async Task<IActionResult> ListPublished()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Published).Select(e => Mapper.Map<EventViewModel>(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("deleted")]
    public async Task<IActionResult> ListDeleted()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Deleted).Select(e => Mapper.Map<EventViewModel>(e)).ToListAsync();
      return Ok(events);
    }


    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetEvent")]
    public async Task<IActionResult> Get(int id)
    {
      var @event = await DbContext.Events
          .Include(e =>e.EventReports).ThenInclude(er => er.Report)
          .SingleOrDefaultAsync(e => e.Id == id);

      if (@event == null)
      {
        return NotFound();
      }

      if (User.Identity.IsAuthenticated || @event.State == Models.EntityState.Published)
      {
        return Ok(Mapper.Map<EventViewModel>(@event));
      }

      return Unauthorized();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      if (@event.Delete())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //Should not happen.
    }


    //[HttpPost]
    //public async Task<IActionResult> Undelete(int id)
    //{
    //  var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
    //  if (@event == null)
    //  {
    //    return NotFound();
    //  }

    //  if (@event.UnDelete())
    //  {
    //    await DbContext.SaveChangesAsync();
    //    return CreatedAtRoute("GetEvent", new { @event.Id }, @event);
    //  }

    //  return BadRequest(); //TODO Error messages.
    //}


    [SuppressValidateModel]
    [HttpPost("published")]
    public async Task<IActionResult> Publish([FromBody]EditEventViewModel model)
    {
      if (model.Id > 0)
      {
        //Publish existing.
        var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == model.Id);
        if (@event == null)
        {
          return NotFound();
        }

        if (@event.State == Models.EntityState.Published)
        {
          return NoContent();
        }

        if (@event.Publish())
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

        var @event = Mapper.Map<Event>(model);
        if (@event.Publish())
        {
          await DbContext.Events.AddAsync(@event);
          await DbContext.SaveChangesAsync();
          return CreatedAtRoute("GetEvent", new { @event.Id }, Mapper.Map<EventViewModel>(@event));
        }
      }

      return BadRequest(); //TODO Error messages.
    }


    [HttpDelete("published/{id:int}")]
    public async Task<IActionResult> Unpublish(int id)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      if (@event.UnPublish())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //TODO Error messages.
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateEventViewModel model)
    {
      var @event = Mapper.Map<Event>(model);

      await DbContext.Events.AddAsync(@event);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetEvent", new { @event.Id }, Mapper.Map<EventViewModel>(@event));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody]EditEventViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      Mapper.Map(model, @event);

      await DbContext.SaveChangesAsync();

      return Ok(Mapper.Map<EventViewModel>(@event));
    }


    [AllowAnonymous]
    [HttpGet("{id:int}/reports")]
    public async Task<IActionResult> ListReports(int id)
    {
      var @event = await DbContext.Events
        .Include(e => e.EventReports).ThenInclude(er => er.Report)
        .SingleOrDefaultAsync(e => e.Id == id);

      if (@event == null)
      {
        return NotFound();
      }

      if (User.Identity.IsAuthenticated || @event.State == Models.EntityState.Published)
      {
        if (User.Identity.IsAuthenticated)
        {
          return ListReportsUndeletedAsync(@event);
        }
        else
        {
          return ListReportsPublished(@event);
        }
      }

      return Unauthorized();
    }


    private IActionResult ListReportsUndeletedAsync(Event @event)
    {
      var reports = @event.EventReports.Where(er => er.Report.State != Models.EntityState.Deleted).Select(r => Mapper.Map<ListReportViewModel>(r.Report)).ToList();
      return Ok(reports);
    }


    private IActionResult ListReportsPublished(Event @event)
    {
      var reports = @event.EventReports.Where(er => er.Report.State == Models.EntityState.Published).Select(r => Mapper.Map<ListReportViewModel>(r.Report)).ToList();
      return Ok(reports);
    }


    [HttpPost("{id:int}/reports")]
    public async Task<IActionResult> PostReport(int id, [FromBody]CreateReportViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var report = Mapper.Map<Report>(model);
      @event.EventReports.Add(new EventReport { Report = report });
      await DbContext.SaveChangesAsync();

      return CreatedAtRoute("GetReport", new { id = report.Id }, Mapper.Map<ReportViewModel>(report));
    }


    //Does not delete the report. Just removes in from this event.
    [HttpDelete("{id:int}/reports/{reportId:int}")]
    public async Task<IActionResult> DeleteReport(int id, int reportId)
    {
      var @event = await DbContext.Events.Include(e => e.EventReports).SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var eventReport = @event.EventReports.SingleOrDefault(er => er.ReportId == reportId);
      if (eventReport == null)
      {
        return NotFound();
      }

      @event.EventReports.Remove(eventReport);
      await DbContext.SaveChangesAsync();

      return NoContent();
    }


    [HttpPost("{id:int}/images")]
    public async Task<ActionResult> PostImage(int id, [FromBody]CreateImageViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var image = Mapper.Map<Image>(model);
      @event.EventImages.Add(new EventImage { Image = image });
      await DbContext.SaveChangesAsync();

      // return Ok(new { Location = Url.Action("Get", "Image", new { id = image.Id })});
      return CreatedAtRoute("GetImage", new { id = image.Id });
    }


    [HttpPost("{id:int}/images/{imageId:Guid}")]
    public async Task<ActionResult> DeleteImage(int id, int imageId)
    {
      var @event = await DbContext.Events.Include(e => e.EventReports).SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var eventImage = @event.EventImages.SingleOrDefault(er => er.ImageId == imageId);
      if (eventImage == null)
      {
        return NotFound();
      }

      eventImage.Image.Delete();
      @event.EventImages.Remove(eventImage);
      await DbContext.SaveChangesAsync();

      return NoContent();
    }
  }
}
