using System;
using System.Collections.Generic;

namespace zulu.Models
{
  public class Member : Entity
  {
    public Member()
    {
    }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Alias { get; set; }
    public string Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public MemberPosition Position { get; set; }
    public MemberRank Rank { get; set; }

    public DateTime? Joined { get; set; }
    public DateTime? Invested { get; set; }
    public DateTime? Left { get; set; }
  }
}