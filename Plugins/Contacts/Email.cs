﻿#if _CONTACTS_

namespace AD.Plugins.Contacts
{
  public enum EmailType
  {
    Home,
    Work,
    Other
  }

  public class Email
  {
    public EmailType Type
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public string Address
    {
      get;
      set;
    }
  }
}

#endif