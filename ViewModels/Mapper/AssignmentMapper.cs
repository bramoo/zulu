using System;
using System.Collections.Generic;
using System.Linq;
using zulu.Data;
using zulu.Models;

namespace zulu.ViewModels.Mapper
{
  public abstract class AssignmentMapper<TAssignment, TViewModel> :
    IMapper<TAssignment, TViewModel>
      where TAssignment : Assignment
      where TViewModel : AssignmentViewModel<TAssignment>
  {
    public AssignmentMapper(AppDbContext dbContext, MemberMapper memberMapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      MemberMapper = memberMapper ?? throw new ArgumentNullException(nameof(memberMapper));
    }

    private AppDbContext DbContext { get; }
    private MemberMapper MemberMapper { get; }

    protected abstract TViewModel CreateAndMap(TAssignment src);

    public TViewModel Map(TAssignment src)
    {
      var vm = CreateAndMap(src);

      vm.AssignmentType = src.AssignmentType;
      vm.Description = src.Description;
      vm.DueDate = src.DueDate;
      vm.CompletionDate = src.CompletionDate;

      vm.Owner = MemberMapper.Map(src.Owner);
      vm.Assignee = MemberMapper.Map(src.Assignee);
      vm.Followers = new SortedSet<MemberViewModel>(src.Followers.Select(f => MemberMapper.Map(f)));

      return vm;
    }

    protected abstract void UpdateExtra(TAssignment dest, TViewModel src);

    public TAssignment Update(TAssignment dest, TViewModel src)
    {
      dest.AssignmentType = src.AssignmentType;
      dest.Description = src.Description;
      dest.DueDate = src.DueDate;
      dest.CompletionDate = src.CompletionDate;

      dest.OwnerId = src.Owner.Id;
      dest.AssigneeId = src.Assignee.Id;

      dest.Followers.Clear();
      foreach (var follower in src.Followers)
      {
        dest.Followers.Add(DbContext.Members.First(m => m.Id == follower.Id));
      }

      UpdateExtra(dest, src);

      return dest;
    }
  }
}