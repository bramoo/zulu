using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using zulu.ViewModels.ValueResolvers;

namespace zulu.ViewModels.Image
{
  public class CreateImageViewModel
  {
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string Description { get; set; }
  }


  public class CreateImageViewModelValidator : AbstractValidator<CreateImageViewModel>
  {
    private static IList<string> ValidImageContentTypes = new List<string> { "image/jpeg", "image/png", "iimage/gif" };
    // RuleFor(x => x.Parameter).Must(x => Conditions.Contains(x)).WithMessage("Please only use: " + String.Join(",", Conditions);

    public CreateImageViewModelValidator()
    {
      RuleFor(e => e.FileName).NotEmpty();
      RuleFor(e => e.ContentType).NotEmpty().Must(i => ValidImageContentTypes.Contains(i));
      // RuleFor(e => e.Description).NotEmpty();
    }
  }


  public class CreateImageViewModelProfile : Profile
  {
    public CreateImageViewModelProfile()
    {
      CreateMap<CreateImageViewModel, zulu.Models.Image>()
        .ForMember(vm => vm.ContentType, opt => opt.ResolveUsing<ContentTypeValueResolver>());
    }
  }
}