#if _CONTACTS_

namespace AD.Plugins.Contacts
{
  public enum RelationshipType
  {
    SignificantOther,
    Child,
    Other
  }

  public class Relationship
  {
    public string Name
    {
      get;
      set;
    }

    public RelationshipType Type
    {
      get;
      set;
    }
  }
}

#endif