using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace zulu.Models
{
	public enum EntityState
  {
    Draft,
    Published,
    Deleted,
    Created
  }

  public abstract class Entity
  {
    public Entity()
    {
      Created = Modified = DateTime.Now;
      State = EntityState.Created;
    }

    public int Id { get; set; }

    [BindNever]
    [JsonConverter(typeof(StringEnumConverter))]
    public EntityState State { get; set; }

    [BindNever]
    public DateTime Created { get; set; }

    [BindNever]
    public DateTime Modified { get; set; }


    public bool Delete()
    {
      State = EntityState.Deleted;
      return true;
    }


    public virtual bool UnDelete()
    {
      if (State == EntityState.Deleted)
      {
        State = EntityState.Created;
        return true;
      }

      return false;
    }
  }
}
