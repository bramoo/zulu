using System;
using System.Linq;
using zulu.Models;

namespace zulu.ViewModels.Mapper
{
  public class EventMapper :
      IMapper<Models.Event, EventViewModel>,
      IMapper<Models.Event, FullEventViewModel>
  {
    public EventMapper(EventAttendanceMapper attendanceMapper, ReportMapper reportMapper, ImageDescriptionMapper imageMapper)
    {
      AttendanceMapper = attendanceMapper ?? throw new ArgumentNullException(nameof(attendanceMapper));
      ReportMapper = reportMapper ?? throw new ArgumentNullException(nameof(reportMapper));
      ImageMapper = imageMapper ?? throw new ArgumentNullException(nameof(imageMapper));
    }


    private ReportMapper ReportMapper { get; }

    private ImageDescriptionMapper ImageMapper { get; }

    private EventAttendanceMapper AttendanceMapper { get; }

    public EventViewModel Map(Event src)
    {
      return new EventViewModel
      {
        Id = src.Id,
        State = src.State.ToString(),
        Created = src.Created,
        LastModified = src.Modified,

        Name = src.Name,
        AllDay = src.AllDay,
        Start = src.Start,
        End = src.End,
      };
    }


    public FullEventViewModel MapFull(Event src)
    {
      return new FullEventViewModel
      {
        Id = src.Id,
        State = src.State.ToString(),
        Created = src.Created,
        LastModified = src.Modified,

        Name = src.Name,
        AllDay = src.AllDay,
        Start = src.Start,
        End = src.End,

        Attendance = src.Attendance.Select(a => AttendanceMapper.Map(a)),
        Reports = src.Reports.Select(r => ReportMapper.Map(r)),
        Images = src.Images.Select(i => ImageMapper.Map(i)),
      };
    }


    public Event Update(Event dest, EventViewModel src)
    {
      dest.Name = src.Name;
      dest.AllDay = src.AllDay;
      dest.Start = src.Start;
      dest.End = src.End;

      return dest;
    }

    FullEventViewModel IMapper<Event, FullEventViewModel>.Map(Event src)
    {
      return MapFull(src);
    }


    public Event Update(Event dest, FullEventViewModel src)
    {
      Update(dest, (EventViewModel)src);

      Mapper.Merge(dest.Reports, src.Reports, ReportMapper);
      Mapper.Merge(dest.Images, src.Images, ImageMapper);

      return dest;
    }
  }
}