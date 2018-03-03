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
    [HttpGet("", Name = "AllEvents")]
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


    [NonAction]
    public async Task<IActionResult> ListUndeleted()
    {
      var entities = await DbContext.Events.Where(e => e.State != Models.EntityState.Deleted).ToListAsync();
      return Ok(entities);
    }


    [HttpGet("draft", Name = "DraftEvents")]
    public async Task<IActionResult> ListDraft()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Draft).ToListAsync();
      return Ok(events);
    }


    [HttpGet("published", Name = "PublishedEvents")]
    public async Task<IActionResult> ListPublished()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Published).ToListAsync();
      return Ok(events);
    }


    [HttpGet("deleted", Name = "DeletedEvents")]
    public async Task<IActionResult> ListDeleted()
    {
      var events = await DbContext.Events.Where(e => e.State == Models.EntityState.Deleted).ToListAsync();
      return Ok(events);
    }


    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetEvents")]
    public async Task<IActionResult> Get(int id)
    {
      var @event = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (@event == null)
      {
        return NotFound();
      }

      if (User.Identity.IsAuthenticated || @event.State == Models.EntityState.Published)
      {
        return Ok(@event);
      }

      return Unauthorized();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var entity = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (entity == null)
      {
        return NotFound();
      }

      if (entity.Delete())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //Should not happen.
    }


    [HttpPost("", Name = "UndeleteEvents")]
    public async Task<IActionResult> Undelete(int id)
    {
      var entity = await DbContext.Events.SingleOrDefaultAsync(e => e.Id == id);
      if (entity == null)
      {
        return NotFound();
      }

      if (entity.UnDelete())
      {
        await DbContext.SaveChangesAsync();
        return CreatedAtAction("Get", new { entity.Id }, entity);
      }

      return BadRequest(); //TODO Error messages.
    }


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
          return CreatedAtAction("Get", new { @event.Id }, Mapper.Map<EventViewModel>(@event));
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


    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody]CreateEventViewModel model)
    {
      var @event = Mapper.Map<Event>(model);

      await DbContext.Events.AddAsync(@event);
      await DbContext.SaveChangesAsync();
      return CreatedAtAction("Get", new { @event.Id }, Mapper.Map<EventViewModel>(@event));
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

      return Ok(@event);
    }
  }
}
