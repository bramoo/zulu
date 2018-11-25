using System;

namespace zulu.Extensions
{
  public class ControllerNameAttribute : Attribute
  {
    public ControllerNameAttribute(string name)
    {
      Name = name;
    }

    public string Name { get; }
  }
}