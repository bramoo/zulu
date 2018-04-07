using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Member;

namespace zulu.ViewModels.ValueResolvers
{
  public class MemberRankValueResolver 
    : IValueResolver<MemberViewModel, zulu.Models.Member, MemberRank>,
      IValueResolver<zulu.Models.Member, MemberViewModel, string>
  {
    public MemberRankValueResolver(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public AppDbContext DbContext { get; }


    public MemberRank Resolve(MemberViewModel source, Models.Member destination, MemberRank destMember, ResolutionContext context)
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


    public string Resolve(Models.Member source, MemberViewModel destination, string destMember, ResolutionContext context)
    {
      return source.Rank?.Rank;
    }
  }
}