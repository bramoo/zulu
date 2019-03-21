using System;

namespace zulu.Attributes
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