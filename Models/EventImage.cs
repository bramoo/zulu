using System;

namespace zulu.Models
{
  public class EventImage
  {
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int ImageId { get; set; }
    public Image Image { get; set; }
  }
}