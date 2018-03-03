using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Models;

namespace zulu.Controllers
{
	[Route("api/v1/{controller}")]
  public abstract class EntityController<T> : Controller where T : Entity
  {
    protected AppDbContext DbContext { get; }
    protected abstract DbSet<T> Entities { get; }


    public EntityController(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    [NonAction]
    protected async Task<IActionResult> List()
    {
      var entities = await Entities.Where(e => e.State != Models.EntityState.Deleted).ToListAsync();
      return Ok(entities);
    }


    [NonAction]
    protected async Task<IActionResult> ListDraft()
    {
      var entities = await Entities.Where(e => e.State == Models.EntityState.Draft).ToListAsync();
      return Ok(entities);
    }


    [NonAction]
    protected async Task<IActionResult> ListPublished()
    {
      var entities = await Entities.Where(e => e.State == Models.EntityState.Published).ToListAsync();
      return Ok(entities);
    }


    [NonAction]
    protected async Task<IActionResult> ListDeleted()
    {
      var entities = await Entities.Where(e => e.State == Models.EntityState.Deleted).ToListAsync();
      return Ok(entities);
    }


    [NonAction]
    protected async Task<IActionResult> Get(int id)
    {
      var entity = await Entities.SingleOrDefaultAsync(e => e.Id == id);
      if (entity == null)
      {
        return NotFound();
      }

      return Ok(entity);
    }


    [NonAction]
    protected async Task<IActionResult> Delete(int id)
    {
      var entity = await Entities.SingleOrDefaultAsync(e => e.Id == id);
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


    [NonAction]
    protected async Task<IActionResult> Undelete(int id)
    {
      var entity = await Entities.SingleOrDefaultAsync(e => e.Id == id);
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


    [NonAction]
    protected async Task<IActionResult> Publish(T model)
    {
      var entity = await Entities.SingleOrDefaultAsync(e => e.Id == model.Id);
      if (entity == null)
      {
        return NotFound();
      }

      if (entity.Publish())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //TODO Error messages.
    }


    [NonAction]
    protected async Task<IActionResult> Unpublish(int id)
    {
      var entity = await Entities.SingleOrDefaultAsync(e => e.Id == id);
      if (entity == null)
      {
        return NotFound();
      }

      if (entity.UnPublish())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //TODO Error messages.
    }


    [NonAction]
    protected async Task<IActionResult> Post(T model)
    {
      await Entities.AddAsync(model);
      await DbContext.SaveChangesAsync();
      return CreatedAtAction("Get", new { model.Id }, model);
    }
  }
}
