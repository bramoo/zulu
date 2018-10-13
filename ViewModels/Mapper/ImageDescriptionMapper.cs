using System;
using System.Linq;
using zulu.Data;
using zulu.Models;

namespace zulu.ViewModels.Mapper
{
  public class ImageDescriptionMapper 
    : IMapper<Models.Image, ImageDescriptionViewModel>
  {
    public ImageDescriptionMapper(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private AppDbContext DbContext { get; }


    public ImageDescriptionViewModel Map(Models.Image src)
    {
      return new ImageDescriptionViewModel
      {
        Id = src.Id,
        State = src.State.ToString(),
        Created = src.Created,
        LastModified = src.Modified,

        FileName = src.FileName,
        ContentType = src.ContentType.Name,
        Description = src.Description,
      };
    }
    

    public Models.Image Update(Models.Image dest, ImageDescriptionViewModel src)
    {
      dest.FileName = src.FileName;
      dest.ContentType = ResolveContentType(src);
      dest.Description = src.Description;

      return dest;
    }


    public Models.ContentType ResolveContentType(ImageDescriptionViewModel source)
    {
      return DbContext.ContentTypes.Single(ct => ct.Name == source.ContentType);
    }
  }
}