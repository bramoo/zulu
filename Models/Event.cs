using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace zulu.Models
{
  public class Event : PublishableEntity
  {
    public Event()
    {
      Attendance = new List<EventAttendance>();
      EventImages = new List<EventImage>();
      EventReports = new List<EventReport>();
    }

    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public ICollection<EventAttendance> Attendance { get; set; }
    public ICollection<EventReport> EventReports { get; set; }
    public ICollection<EventImage> EventImages { get; set; }


    [NotMapped]
    public IEnumerable<Report> Reports
    {
      get
      {
        return EventReports?.Select(er => er.Report);
      }
    }

    [NotMapped]
    public IEnumerable<Image> Images
    {
      get
      {
        return EventImages?.Select(ei => ei.Image);
      }
    }
  }
}
