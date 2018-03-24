using System;

namespace zulu.Models
{
  public class Image : Entity
  {
    public string FileName { get; set; }
    public ContentType ContentType { get; set; }
    public string Description { get; set; }
  }
}