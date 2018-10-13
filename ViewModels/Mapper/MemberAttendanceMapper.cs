using System;
using System.Linq;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;

namespace zulu.ViewModels.Mapper
{
  public class MemberAttendanceMapper : IMapper<Models.EventAttendance, MemberAttendanceViewModel>
  {
    public MemberAttendanceMapper(AppDbContext dbContext, EventMapper eventMapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      EventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
    }


    private AppDbContext DbContext { get; }
    private EventMapper EventMapper { get; }


    public MemberAttendanceViewModel Map(EventAttendance src)
    {
      return new MemberAttendanceViewModel
      {
        Event = EventMapper.Map(src.Event),
        Attended = src.Attended,
        ServiceHours = src.ServiceHours,
      };
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