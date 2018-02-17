using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class EventListViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public bool Deleted { get; set; }
  }
}
