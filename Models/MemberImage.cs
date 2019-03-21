using System;

namespace zulu.Models
{
  public class MemberImage
  {
    public int MemberId { get; set; }
    public Member Member { get; set; }

    public int ImageId { get; set; }
    public Image Image { get; set; }
  }
}