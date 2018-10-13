using System.Collections.Generic;
using System.Linq;
using zulu.Models;

namespace zulu.ViewModels.Mapper
{
  public interface IMapper<TEntity, TViewModel>
  {
    TViewModel Map(TEntity src);
    TEntity Update(TEntity dest, TViewModel src);
  }

  public static class Mapper
  {
    public static void Merge<TEntity, TViewModel>(IEnumerable<TEntity> entities, IEnumerable<TViewModel> viewModels, IMapper<TEntity, TViewModel> mapper)
        where TEntity : Entity, new()
        where TViewModel : EntityViewModel
    {
      foreach (var entity in entities.Where(m => !viewModels.Any(vm => m.Id == vm.Id)))
      {
        entity.Delete();
      }
      var entityDictionary = entities.ToDictionary(m => m.Id);
      foreach (var viewModel in viewModels)
      {
        if (!entityDictionary.ContainsKey(viewModel.Id))
        {
          entityDictionary[viewModel.Id] = new TEntity();
        }
        mapper.Update(entityDictionary[viewModel.Id], viewModel);
      }
    }
  }
}