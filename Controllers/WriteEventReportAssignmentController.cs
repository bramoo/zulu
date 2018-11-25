using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Extensions;
using zulu.Models;
using zulu.Models.Assignments;
using zulu.ViewModels;
using zulu.ViewModels.Assignments;
using zulu.ViewModels.Mapper;
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
