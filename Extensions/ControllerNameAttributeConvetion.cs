using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace zulu.Extensions{
  public class ControllerNameAttributeConvention : IControllerModelConvention
  {
    public void Apply(ControllerModel controller)
    {
      var controllerNameAttribute = controller.Attributes.OfType<ControllerNameAttribute>().SingleOrDefault();
      if(controllerNameAttribute != null)
      {
        controller.ControllerName = controllerNameAttribute.Name;
      }
    }
  }
}