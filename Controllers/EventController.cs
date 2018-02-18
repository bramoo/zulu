using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/events")]
  public class EventController : EntityController<Event>
  {
    //private static List<Event> events;

    //static EventController()
    //{
    //  events = new List<Event>
    //  {
    //    new Event {Id = 1, Name = "Event 1", Start = DateTime.Today, End = DateTime.Today.AddDays(1), AllDay = true },
    //    new Event {Id = 2, Name = "Event 2", Start = DateTime.Today.AddHours(9), End = DateTime.Today.AddHours(12), AllDay = false },
    //    new Event {Id = 3, Name = "Event 3", Start = DateTime.Today, End = DateTime.Today.AddDays(2), AllDay = true },
    //  };
    //}

    public EventController(AppDbContext dbContext)
      :base(dbContext)
    {
    }

    protected override DbSet<Event> Entities => DbContext.Events;


    [HttpGet("", Name = "AllEvents")]
    public new async Task<IActionResult> List() => await base.List();


    [HttpGet("draft", Name = "DraftEvents")]
    public new async Task<IActionResult> ListDraft() => await base.ListDraft();


    [HttpGet("published", Name = "PublishedEvents")]
    public new async Task<IActionResult> ListPublished() => await base.ListPublished();


    [HttpGet("draft", Name = "DeletedEvents")]
    public new async Task<IActionResult> ListDeleted() => await base.ListDeleted();


    [HttpGet("{id:int}", Name = "GetEvents")]
    public new async Task<IActionResult> Get(int id) => await base.Get(id);


    [HttpDelete("{id:int}")]
    public new async Task<IActionResult> Delete(int id) => await base.Delete(id);


    [HttpPost("", Name = "UndeleteEvents")]
    public new async Task<IActionResult> Undelete([FromBody]int id, [FromBody]Event model) => await base.Undelete(id, model);


    [HttpPost("published")]
    public new async Task<IActionResult> Publish([FromBody]int id) => await base.Publish(id);


    [HttpDelete("published/{id:int}")]
    public new async Task<IActionResult> Unpublish(int id) => await base.Unpublish(id);




    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody]Event model)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if(@event == null)
      {
        return NotFound();
      }

      @event.Name = model.Name;
      @event.Start = model.Start;
      @event.End = model.End;
      @event.AllDay = model.AllDay;

      await DbContext.SaveChangesAsync();

      return Ok(@event);
    }
  }
}
