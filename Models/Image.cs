using System;

namespace zulu.Models
{
  public class Image : PublishableEntity
  {
    public string FileName { get; set; }
    public string DisplayName { get; set; }
    public ContentType ContentType { get; set; }
    public string Description { get; set; }
  }
}