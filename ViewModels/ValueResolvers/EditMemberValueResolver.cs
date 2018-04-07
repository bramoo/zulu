using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Member;

namespace zulu.ViewModels.ValueResolvers
{
  public class EditMemberValueResolver 
    : IValueResolver<EditMemberViewModel, zulu.Models.Member, MemberRank>,
      IValueResolver<EditMemberViewModel, zulu.Models.Member, MemberPosition>
  {
    public EditMemberValueResolver(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public AppDbContext DbContext { get; }

    public MemberRank Resolve(EditMemberViewModel source, Models.Member destination, MemberRank destMember, ResolutionContext context)
    {
      var rank = DbContext.MemberRanks.SingleOrDefault(mr => mr.Rank == source.Rank);
      if (rank == null)
      {
        rank = new MemberRank { Rank = source.Rank };
        DbContext.MemberRanks.Add(rank);
        DbContext.SaveChanges();
      }
      return rank;
    }

    public MemberPosition Resolve(EditMemberViewModel source, Models.Member destination, MemberPosition destMember, ResolutionContext context)
    {
      var position = DbContext.MemberPositions.SingleOrDefault(mr => mr.Name == source.Position);
      if (position == null)
      {
        position = new MemberPosition { Name = source.Position };
        DbContext.MemberPositions.Add(position);
        DbContext.SaveChanges();
      }
      return position;
    }
  }
}