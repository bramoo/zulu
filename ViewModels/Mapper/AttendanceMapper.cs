using System;
using System.Linq;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;

namespace zulu.ViewModels.Mapper
{
  public class AttendanceMapper :
      IMapper<Models.EventAttendance, EventAttendanceViewModel>,
      IMapper<Models.EventAttendance, MemberAttendanceViewModel>
  {
    public AttendanceMapper(AppDbContext dbContext, MemberMapper memberMapper, EventMapper eventMapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      EventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
      MemberMapper = memberMapper ?? throw new ArgumentNullException(nameof(memberMapper));
    }

    private AppDbContext DbContext { get; }
    private EventMapper EventMapper { get; }
    private MemberMapper MemberMapper { get; }


    EventAttendanceViewModel IMapper<EventAttendance, EventAttendanceViewModel>.Map(EventAttendance src)
    {
      return new EventAttendanceViewModel
      {
        Member = MemberMapper.Map(src.Member),
        Attended = src.Attended,
        ServiceHours = src.ServiceHours,
      };
    }


    MemberAttendanceViewModel IMapper<EventAttendance, MemberAttendanceViewModel>.Map(EventAttendance src)
    {
      return new MemberAttendanceViewModel
      {
        Event = EventMapper.Map(src.Event),
        Attended = src.Attended,
        ServiceHours = src.ServiceHours,
      };
    }


    public EventAttendance Update(EventAttendance dest, EventAttendanceViewModel src)
    {
      dest.Member = DbContext.Members.Single(m => m.Id == src.Member.Id);
      dest.Attended = src.Attended;
      dest.ServiceHours = src.ServiceHours;

      return dest;
    }

    public EventAttendance Update(EventAttendance dest, MemberAttendanceViewModel src)
    {
      dest.Event = DbContext.Events.Single(m => m.Id == src.Event.Id);
      dest.Attended = src.Attended;
      dest.ServiceHours = src.ServiceHours;

      return dest;
    }
  }
}