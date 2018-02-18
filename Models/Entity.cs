﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models
{
  public enum EntityState
  {
    Draft,
    Published,
    Deleted
  }

  public class Entity
  {
    public Entity()
    {
      Created = Modified = DateTime.Now;
    }


    [BindNever]
    public int Id { get; set; }

    [BindNever]
    public EntityState State { get; set; }

    [BindNever]
    public DateTime Created { get; set; }

    [BindNever]
    public DateTime Modified { get; set; }


    public bool Publish()
    {
      if(State == EntityState.Draft)
      {
        State = EntityState.Published;
        return true;
      }

      return false;
    }


    public bool UnPublish()
    {
      if(State == EntityState.Published)
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
      if(State == EntityState.Deleted)
      {
        State = EntityState.Draft;
        return true;
      }

      return false;
    }
  }
}
