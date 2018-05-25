using System;


namespace zulu.ViewModels
{
  public class EntityViewModel
  {
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
  }
}