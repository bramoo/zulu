using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using zulu.Attributes;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;
using zulu.ViewModels.Mapper;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/events")]
  [Authorize]
  public class EventController : Controller
  {
    public EventController(AppDbContext dbContext, EventMapper eventMapper, ReportMapper reportMapper, ImageDescriptionMapper imageDescriptionMapper, IMapper<EventAttendance, EventAttendanceViewModel> attendanceMapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      EventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
      ReportMapper = reportMapper ?? throw new ArgumentNullException(nameof(reportMapper));
      ImageDescriptionMapper = imageDescriptionMapper ?? throw new ArgumentNullException(nameof(imageDescriptionMapper));
      AttendanceMapper = attendanceMapper ?? throw new ArgumentNullException(nameof(attendanceMapper));
    }


    private AppDbContext DbContext { get; }
    private EventMapper EventMapper { get; }
    private ReportMapper ReportMapper { get; }
    private ImageDescriptionMapper ImageDescriptionMapper { get; }
    private IMapper<EventAttendance, EventAttendanceViewModel> AttendanceMapper { get; }


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
      var events = await DbContext.Events.Where(e => e.State != Models.EntityState.Deleted).Select(e => EventMapper.Map(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("draft")]
    public async Task<IActionResult> ListDraft()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Draft).Select(e => EventMapper.Map(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("published")]
    public async Task<IActionResult> ListPublished()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Published).Select(e => EventMapper.Map(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("deleted")]
    public async Task<IActionResult> ListDeleted()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Deleted).Select(e => EventMapper.Map(e)).ToListAsync();
      return Ok(events);
    }


    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetEvent")]
    public async Task<IActionResult> Get(int id)
    {
      //TODO: only get published reports/images if user is not authenticated.

      var @event = await DbContext.Events
          .Include(e => e.EventReports).ThenInclude(er => er.Report)
          .Include(e => e.EventImages).ThenInclude(ei => ei.Image).ThenInclude(i => i.ContentType)
          .Include(e => e.Attendance).ThenInclude(ea => ea.Member)
          .SingleOrDefaultAsync(e => e.Id == id);

      if (@event == null)
      {
        return NotFound();
      }

      if (User.Identity.IsAuthenticated || @event.State == Models.EntityState.Published)
      {
        return Ok(EventMapper.MapFull(@event));
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
    public async Task<IActionResult> Publish([FromBody]FullEventViewModel model)
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

        var @event = EventMapper.Update(new Event(), model);
        if (@event.Publish())
        {
          await DbContext.Events.AddAsync(@event);
          await DbContext.SaveChangesAsync();
          return CreatedAtRoute("GetEvent", new { @event.Id }, EventMapper.MapFull(@event));
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
    public async Task<IActionResult> Post([FromBody]FullEventViewModel model)
    {
      var @event = EventMapper.Update(new Event(), model);

      await DbContext.Events.AddAsync(@event);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetEvent", new { @event.Id }, EventMapper.MapFull(@event));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]FullEventViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      EventMapper.Update(@event, model);

      await DbContext.SaveChangesAsync();

      return Ok(EventMapper.MapFull(@event));
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
      var reports = @event.EventReports.Where(er => er.Report.State != Models.EntityState.Deleted).Select(r => ReportMapper.Map(r.Report)).ToList();
      return Ok(reports);
    }


    private IActionResult ListReportsPublished(Event @event)
    {
      var reports = @event.EventReports.Where(er => er.Report.State == Models.EntityState.Published).Select(r => ReportMapper.Map(r.Report)).ToList();
      return Ok(reports);
    }


    [HttpPost("{id:int}/reports")]
    public async Task<IActionResult> PostReport(int id, [FromBody]ReportViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var report = ReportMapper.Update(new Report(), model);
      @event.EventReports.Add(new EventReport { Report = report });
      await DbContext.SaveChangesAsync();

      return CreatedAtRoute("GetReport", new { id = report.Id }, ReportMapper.Map(report));
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
    public async Task<ActionResult> PostImage(int id, [FromBody]ImageDescriptionViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var image = ImageDescriptionMapper.Update(new Image(), model);
      @event.EventImages.Add(new EventImage { Image = image });
      await DbContext.SaveChangesAsync();

      return CreatedAtRoute("GetImage", new { id = image.Id });
    }


    [HttpPost("{id:int}/images/{imageId:int}")]
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


    [HttpGet("{id:int}/attendance")]
    public async Task<IActionResult> ListAttendance(int id)
    {
      var @event = await DbContext.Events
        .Include(e => e.Attendance).ThenInclude(ea => ea.Member)
        .SingleOrDefaultAsync(e => e.Id == id);

      if (@event == null)
      {
        return NotFound();
      }

      var attendance = @event.Attendance.Select(a => AttendanceMapper.Map(a));
      return Ok(attendance);
    }


    [HttpPost("{id:int}/attendance")]
    public async Task<IActionResult> PostAttendance(int id, [FromBody]EventAttendanceViewModel model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound($"Event '{id}' not found.");
      }

      var attendance = @event.Attendance.SingleOrDefault(a => a.MemberId == model.Member.Id);
      if (attendance == null)
      {
        attendance = AttendanceMapper.Update(new EventAttendance(), model);
        @event.Attendance.Add(attendance);
      }
      else
      {
        AttendanceMapper.Update(attendance, model);
      }
      await DbContext.SaveChangesAsync();

      return NoContent();
    }


    [HttpDelete("{id:int}/attendance/{memberId:int}")]
    public async Task<IActionResult> DeleteAttendance(int id, int memberId)
    {
      var attendance = await DbContext.Attendance.SingleOrDefaultAsync(a => a.EventId == id && a.MemberId == memberId);
      if (attendance == null)
      {
        return NotFound();
      }

      DbContext.Attendance.Remove(attendance);
      await DbContext.SaveChangesAsync();

      return NoContent();
    }
  }
}
