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
  [Route("api/v1/assignments/[controller`]")]
  public abstract class AssignmentControllerBase<TAssignment, TAssignmentMapper, TViewModel> : Controller
    where TAssignment : Assignment
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

    [HttpGet("complete")]
    public async Task<IActionResult> ListComplete()
    {
      var assignments = await Assignments
        .Include(a => a.Owner)
        .Include(a => a.Assignee)
        .Include(a => a.Followers)
        .Where(a => a.CompletionDate != null).Select(a => Mapper.Map(a)).ToListAsync();
      return Ok(assignments);
    }
  }
}
