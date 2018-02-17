using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public class Page<T>
  {
    public Page(IEnumerable<T> data, Uri previous, Uri next)
    {
      Data = data;
      Paging = new Paging { Previous = previous, Next = next };
    }

    public IEnumerable<T> Data { get; set; }
    public Paging Paging { get; set; }
  }

  public class Paging
  {
    public Uri Previous { get; set; }
    public Uri Next { get; set; }
  }
}
