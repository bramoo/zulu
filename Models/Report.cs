using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class Report
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public string Author { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime? Published { get; set; }
    public DateTime? Deleted { get; set; }
  }
}
