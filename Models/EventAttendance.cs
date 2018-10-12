namespace zulu.Models
{
  public class EventAttendance
  {
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; }

    public bool Attended { get; set; }
    public decimal? ServiceHours { get; set; }
  }
}