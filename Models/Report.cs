using System.ComponentModel.DataAnnotations.Schema;

namespace zulu.Models
{
	public class Report : PublishableEntity
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }

    public EventReport EventReport { get; set; }

    [NotMapped]
    public Event Event
    {
      get { return EventReport?.Event; }
      set { if(EventReport != null) EventReport.Event = value; }
    }
  }
}
