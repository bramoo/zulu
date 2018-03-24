using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Image;

namespace zulu.ViewModels.ValueResolvers
{
  public class ContentTypeValueResolver : IValueResolver<CreateImageViewModel, zulu.Models.Image, ContentType>
  {
    public ContentTypeValueResolver(AppDbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public AppDbContext DbContext { get; }

    public ContentType Resolve(CreateImageViewModel source, Models.Image destination, ContentType destMember, ResolutionContext context)
    {
      return DbContext.ContentTypes.SingleOrDefault(ct => ct.Name == source.ContentType);
    }
  }
}