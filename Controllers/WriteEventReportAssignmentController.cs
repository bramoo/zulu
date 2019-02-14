using Microsoft.EntityFrameworkCore;
using System.Linq;
using zulu.Attributes;
using zulu.Data;
using zulu.Models.Assignments;
using zulu.ViewModels.Assignments;
using zulu.ViewModels.Mapper.Assignments;

namespace zulu.Controllers
{
  // [Produces("application/json")]
  // [Route("api/v1/assignments/write-event")]
  [ControllerName("write-event")]
  public class WriteEventReportAssignmentController : AssignmentControllerBase<WriteEventReportAssignment, WriteEventReportAssignmentMapper, WriteEventReportAssignmentViewModel>
  {
    public WriteEventReportAssignmentController(AppDbContext dbContext, WriteEventReportAssignmentMapper mapper) : base(dbContext, mapper)
    {
    }


    protected override IQueryable<WriteEventReportAssignment> Assignments => DbContext.WriteEventReportAssignments.Include(a => a.Event);
  }
}
