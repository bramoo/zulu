using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public abstract class Assignment : Entity
  {
    public string AssignmentType { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }

    public int OwnerId { get; set; }
    public Member Owner { get; set; }

    public int AssigneeId { get; set; }
    public Member Assignee { get; set; }

    public ISet<Member> Followers { get; set; }

    public void Complete()
    {
      CompletionDate = DateTime.Now;
    }

    public void Uncomplete()
    {
      CompletionDate = null;
    }
  }
}
