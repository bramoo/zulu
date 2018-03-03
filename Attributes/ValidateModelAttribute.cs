using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace zulu.Attributes
{
	public class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (context.ActionDescriptor.FilterDescriptors.Any(fd => fd.Filter.GetType() == typeof(SuppressValidateModelAttribute)))
      {
        return;
      }

      if (!context.ModelState.IsValid)
      {
        context.Result = new BadRequestObjectResult(context.ModelState);
      }
    }
  }


  public class SuppressValidateModelAttribute : Attribute, IFilterMetadata
  {
  }
}