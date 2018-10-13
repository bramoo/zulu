using FluentValidation;
using System;
using System.Collections.Generic;

namespace zulu.ViewModels
{
  public class ImageDescriptionViewModel : EntityViewModel
  {
    public string DisplayName { get; set; }
    public string ContentType { get; set; }
    public string Description { get; set; }
  }


  public class ImageDescriptionViewModelValidator : AbstractValidator<ImageDescriptionViewModel>
  {
    //TODO: load supported content types from database.
    private static IList<string> ValidImageContentTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };

    public ImageDescriptionViewModelValidator()
    {
      RuleFor(e => e.DisplayName).NotEmpty();
      RuleFor(e => e.ContentType).NotEmpty().Must(i => ValidImageContentTypes.Contains(i));
      // RuleFor(e => e.Description).NotEmpty();
    }
  }
}