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
    Deleted
  }

  public abstract class Entity
  {
    public Entity()
    {
      Created = Modified = DateTime.Now;
    }

    public int Id { get; set; }

    [BindNever]
    [JsonConverter(typeof(StringEnumConverter))]
    public EntityState State { get; set; }

    [BindNever]
    public DateTime Created { get; set; }

    [BindNever]
    public DateTime Modified { get; set; }


    public bool Publish()
    {
      if (State == EntityState.Draft)
      {
        State = EntityState.Published;
        return true;
      }

      return false;
    }


    public bool UnPublish()
    {
      if (State == EntityState.Published)
      {
        State = EntityState.Draft;
        return true;
      }

      return false;
    }


    public bool Delete()
    {
      State = EntityState.Deleted;
      return true;
    }


    public bool UnDelete()
    {
      if (State == EntityState.Deleted)
      {
        State = EntityState.Draft;
        return true;
      }

      return false;
    }
  }
}
