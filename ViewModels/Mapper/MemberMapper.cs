using System;
using System.Linq;
using zulu.Data;
using zulu.ViewModels;

namespace zulu.ViewModels.Mapper
{
  public class MemberMapper : IMapper<Models.Member, MemberViewModel>
  {
    public MemberMapper(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private AppDbContext DbContext { get; }

    public MemberViewModel Map(Models.Member src)
    {
      return new MemberViewModel
      {
        Id = src.Id,
        State = src.State.ToString(),
        Created = src.Created,
        LastModified = src.Modified,

        FirstName = src.FirstName,
        Surname = src.Surname,
        Alias = src.Alias,
        Email = src.Email,
        DateOfBirth = src.DateOfBirth,

        Position = src.Position?.Name,
        Rank = src.Rank?.Rank,

        Joined = src.Joined,
        Invested = src.Invested,
        Left = src.Left,
      };
    }

    public Models.Member Update(Models.Member dest, MemberViewModel src)
    {
      dest.FirstName = src.FirstName;
      dest.Surname = src.Surname;
      dest.Alias = src.Alias;
      dest.Email = src.Email;
      dest.DateOfBirth = src.DateOfBirth;

      dest.Position = ResolvePosition(src);
      dest.Rank = ResolveRank(src);

      dest.Joined = src.Joined;
      dest.Invested = src.Invested;
      dest.Left = src.Left;

      return dest;
    }


    public Models.MemberPosition ResolvePosition(MemberViewModel source)
    {
      var position = DbContext.MemberPositions.SingleOrDefault(mr => mr.Name == source.Position);
      if (position == null)
      {
        position = new Models.MemberPosition { Name = source.Position };
        DbContext.MemberPositions.Add(position);
        DbContext.SaveChanges();
      }
      return position;
    }


    private Models.MemberRank ResolveRank(MemberViewModel source)
    {
      var rank = DbContext.MemberRanks.SingleOrDefault(mr => mr.Rank == source.Rank);
      if (rank == null)
      {
        rank = new Models.MemberRank { Rank = source.Rank };
        DbContext.MemberRanks.Add(rank);
        DbContext.SaveChanges();
      }
      return rank;
    }
  }

}