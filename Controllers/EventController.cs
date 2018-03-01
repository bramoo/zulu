using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/events")]
  [Authorize]
  public class EventController : EntityController<Event>
  {
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


    [HttpGet("deleted", Name = "DeletedEvents")]
    public new async Task<IActionResult> ListDeleted() => await base.ListDeleted();


    [HttpGet("{id:int}", Name = "GetEvents")]
    public new async Task<IActionResult> Get(int id) => await base.Get(id);


    [HttpDelete("{id:int}")]
    public new async Task<IActionResult> Delete(int id) => await base.Delete(id);


    [HttpPost("", Name = "UndeleteEvents")]
    public new async Task<IActionResult> Undelete(int id) => await base.Undelete(id);


    [HttpPost("published")]
    public new async Task<IActionResult> Publish([FromBody]Event model) => await base.Publish(model);


    [HttpDelete("published/{id:int}")]
    public new async Task<IActionResult> Unpublish(int id) => await base.Unpublish(id);


    [HttpDelete("published")]
    public async Task<IActionResult> UnpublishFB([FromBody]Event model) => await base.Unpublish(model.Id);


    [HttpPost("")]
    public new async Task<IActionResult> Post([FromBody]Event model) => await base.Post(model);



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
