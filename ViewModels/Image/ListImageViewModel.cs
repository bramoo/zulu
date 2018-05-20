using AutoMapper;

namespace zulu.ViewModels.Image
{
    public class ListImageViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }

    public class ListImageViewModelProfile : Profile
    {
        public ListImageViewModelProfile()
        {
            CreateMap<zulu.Models.Image, ListImageViewModel>();
        }
    }
}
