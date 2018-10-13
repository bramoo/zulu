using System;
using System.Linq;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;

namespace zulu.ViewModels.Mapper
{
  public class EventAttendanceMapper : IMapper<Models.EventAttendance, EventAttendanceViewModel>
  {
    public EventAttendanceMapper(AppDbContext dbContext, MemberMapper memberMapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      MemberMapper = memberMapper ?? throw new ArgumentNullException(nameof(memberMapper));
    }

    private AppDbContext DbContext { get; }
    private MemberMapper MemberMapper { get; }


    public EventAttendanceViewModel Map(EventAttendance src)
    {
      return new EventAttendanceViewModel
      {
        Member = MemberMapper.Map(src.Member),
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
  }
}