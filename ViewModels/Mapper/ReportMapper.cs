using zulu.Models;

namespace zulu.ViewModels.Mapper
{
  public class ReportMapper : IMapper<Models.Report, ReportViewModel>
  {
    public ReportViewModel Map(Models.Report src)
    {
      return new ReportViewModel
      {
        Id = src.Id,
        State = src.State.ToString(),
        Created = src.Created,
        LastModified = src.Modified,

        Title = src.Title,
        Content = src.Content,
        Author = src.Author,
      };
    }

    public Models.Report Update(Models.Report dest, ReportViewModel src)
    {
      dest.Title = src.Title;
      dest.Content = src.Content;
      dest.Author = src.Author;

      return dest;
    }
  }
}