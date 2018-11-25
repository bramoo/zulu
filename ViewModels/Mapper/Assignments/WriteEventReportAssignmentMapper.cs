using System;
using zulu.Data;
using zulu.Models.Assignments;
using zulu.ViewModels.Assignments;

namespace zulu.ViewModels.Mapper.Assignments
{
  public class WriteEventReportAssignmentMapper : AssignmentMapper<WriteEventReportAssignment, WriteEventReportAssignmentViewModel>
  {
    public WriteEventReportAssignmentMapper(AppDbContext dbContext, MemberMapper memberMapper, EventMapper eventMapper) : base(dbContext, memberMapper)
    {
      EventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
    }

    private EventMapper EventMapper { get; }

    protected override WriteEventReportAssignmentViewModel CreateAndMap(WriteEventReportAssignment src) => new WriteEventReportAssignmentViewModel
    {
      Event = EventMapper.Map(src.Event)
    };

    protected override void UpdateExtra(WriteEventReportAssignment dest, WriteEventReportAssignmentViewModel src)
    {
      var vm = (WriteEventReportAssignmentViewModel)src;
      dest.EventId = vm.Event.Id;
    }
  }
}