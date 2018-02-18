using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class EventReport
  {
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int ReportId { get; set; }
    public Report Report { get; set; }
  }
}
