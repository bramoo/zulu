using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;
using zulu.ViewModels.Mapper;

namespace zulu.Controllers
{
  [Route("api/v1/assignments/[controller]")]
  public abstract class AssignmentControllerBase<TAssignment, TAssignmentMapper, TViewModel> : Controller
    where TAssignment : Assignment, new()
    where TViewModel : AssignmentViewModel<TAssignment>
    where TAssignmentMapper : AssignmentMapper<TAssignment, TViewModel>
  {
    public AssignmentControllerBase(AppDbContext dbContext, TAssignmentMapper mapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    protected AppDbContext DbContext { get; }
    protected TAssignmentMapper Mapper { get; }

    protected abstract IQueryable<TAssignment> Assignments { get; }


    [HttpGet]
    public async Task<IActionResult> ListAll()
    {
      var assignments = await Assignments
        .Include(a => a.Owner)
        .Include(a => a.Assignee)
        .Include(a => a.Followers)
        .Select(a => Mapper.Map(a)).ToListAsync();
      return Ok(assignments);
    }


    [HttpGet("outstanding")]
    public async Task<IActionResult> ListOutstanding()
    {
      var assignments = await Assignments
        .Include(a => a.Owner)
        .Include(a => a.Assignee)
        .Include(a => a.Followers)
        .Where(a => a.CompletionDate == null).Select(a => Mapper.Map(a)).ToListAsync();
      return Ok(assignments);
    }


    [HttpGet("completed")]
    public async Task<IActionResult> ListCompleted()
    {
      var assignments = await Assignments
        .Include(a => a.Owner)
        .Include(a => a.Assignee)
        .Include(a => a.Followers)
        .Where(a => a.CompletionDate != null).Select(a => Mapper.Map(a)).ToListAsync();
      return Ok(assignments);
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody]TViewModel model)
    {
      var assignment = Mapper.Update(new TAssignment(), model);
      await DbContext.Assignments.AddAsync(@assignment);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetAssignment", new { assignment.Id }, Mapper.Map(assignment));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var assignment = await DbContext.Assignments.SingleOrDefaultAsync(e => e.Id == id);
      if (assignment == null)
      {
        return NotFound();
      }

      if (assignment.Delete())
      {
        await DbContext.SaveChangesAsync();
        return NoContent();
      }

      return BadRequest(); //Should not happen.
    }


    [HttpPost("completed")]
    public async Task<IActionResult> PostCompleted([FromBody]TViewModel model)
    {
      var assignment = await DbContext.Assignments.SingleOrDefaultAsync(e => e.Id == model.Id);
      if (assignment == null)
      {
        return NotFound();
      }

      assignment.Complete();

      await DbContext.SaveChangesAsync();
      return NoContent();
    }


    [HttpDelete("completed")]
    public async Task<IActionResult> Uncompleted([FromBody]TViewModel model)
    {
      var assignment = await DbContext.Assignments.SingleOrDefaultAsync(e => e.Id == model.Id);
      if (assignment == null)
      {
        return NotFound();
      }

      assignment.Uncomplete();

      await DbContext.SaveChangesAsync();
      return NoContent();
    }
  }
}
