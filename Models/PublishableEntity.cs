namespace zulu.Models
{
  public class PublishableEntity : Entity
  {
    public PublishableEntity()
    {
      State = EntityState.Draft;
    }


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


    public override bool UnDelete()
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