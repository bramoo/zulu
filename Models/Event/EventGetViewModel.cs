﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class EventGetViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public DateTime? Deleted { get; set; }

    public IEnumerable<ReportGetViewModel> Reports { get; set; }
  }
}
