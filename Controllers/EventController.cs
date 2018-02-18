using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/events")]
  public class EventController : Controller
  {
    private static List<Event> events;

    static EventController()
    {
      events = new List<Event>
      {
        new Event {Id = 1, Name = "Event 1", Start = DateTime.Today, End = DateTime.Today.AddDays(1), AllDay = true },
        new Event {Id = 2, Name = "Event 2", Start = DateTime.Today.AddHours(9), End = DateTime.Today.AddHours(12), AllDay = false },
        new Event {Id = 3, Name = "Event 3", Start = DateTime.Today, End = DateTime.Today.AddDays(2), AllDay = true },
      };
    }

    [HttpGet("")]
    public IActionResult List(bool deleted = false)
    {
      var query = (IEnumerable<Event>)events;
      if(!deleted)
      {
        query = query.Where(e => e.Deleted == null);
      }

      var results = events.Select(e => new EventListViewModel
      {
        Id = e.Id,
        Name = e.Name,
        Start = e.Start,
        End = e.End,
        AllDay = e.AllDay,
        Deleted = e.Deleted.HasValue
      });

      return Ok(results);
    }


    [HttpPost("")]
    public IActionResult Post([FromBody]EventPostViewModel model)
    {
      var now = DateTime.Now;

      var @event = new Event
      {
        Id = events.Max(r => r.Id) + 1, //TODO: don't do this.
        Name = model.Name,
        Start = model.Start,
        End = model.End,
        AllDay = model.AllDay
      };

      events.Add(@event);

      return Created(Url.Link("GetEvent", new { @event.Id }), @event);
    }


    [HttpGet("{id:int}", Name = "GetEvent")]
    public IActionResult Get(int id)
    {
      var @event = events.FirstOrDefault(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      var model = new EventGetViewModel
      {
        Id = @event.Id,
        Name = @event.Name,
        Start = @event.Start,
        End = @event.End,
        AllDay = @event.AllDay,
        Reports = @event.Reports.Select(r => new ReportGetViewModel
        {
          Id = r.Id,
          Title = r.Title,
          Content = r.Content,
          Author = r.Author,
          Created = r.Created,
          LastModified = r.LastModified,
          Deleted = r.Deleted.HasValue,
          Published = r.Published.HasValue
        })
      };

      return base.Ok(model);
    }


    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody]EventPostViewModel model)
    {
      var @event = events.FirstOrDefault(e => e.Id == id);
      if(@event == null)
      {
        return NotFound();
      }

      var now = DateTime.Now;

      @event.Name = model.Name;
      @event.Start = model.Start;
      @event.End = model.End;
      @event.AllDay = model.AllDay;

      return Ok(@event);
    }


    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      var @event = events.FirstOrDefault(e => e.Id == id);
      if(@event == null)
      {
        return NotFound();
      }

      @event.Deleted = DateTime.Now;

      return NoContent();
    }
  }
}
