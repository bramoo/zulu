using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace zulu.Models
{
	public class Event : Entity
  {
    public Event()
    {
      EventReports = new List<EventReport>();
    }

    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public ICollection<EventReport> EventReports { get; set; }

    [NotMapped]
    public IEnumerable<Report> Reports
    {
      get
      {
        return EventReports?.Select(er => er.Report);
      }
    }
  }
}
