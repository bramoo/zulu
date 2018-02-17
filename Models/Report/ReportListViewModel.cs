﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class ReportListViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime LastModified { get; set; }
    public bool Published { get; set; }
    public bool Deleted { get; set; }
  }
}