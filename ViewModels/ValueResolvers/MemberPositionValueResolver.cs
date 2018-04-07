using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Member;

namespace zulu.ViewModels.ValueResolvers
{
  public class MemberPositionValueResolver 
    : IValueResolver<MemberViewModel, zulu.Models.Member, MemberPosition>,
      IValueResolver<zulu.Models.Member, MemberViewModel, string>
  {
    public MemberPositionValueResolver(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public AppDbContext DbContext { get; }

    
    public MemberPosition Resolve(MemberViewModel source, Models.Member destination, MemberPosition destMember, ResolutionContext context)
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


    public string Resolve(Models.Member source, MemberViewModel destination, string destMember, ResolutionContext context)
    {
      return source.Position?.Name;
    }
  }
}