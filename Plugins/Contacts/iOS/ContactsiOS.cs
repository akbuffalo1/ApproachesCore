#if __IOS__ && _CONTACTS_

using System.Linq;
using System.Threading.Tasks;


namespace AD.Plugins.Contacts.iOS
{
	public class ContactsiOS : IContacts
	{
		private AddressBook addressBook;
		private AddressBook AddressBook
		{
			get { return addressBook ?? (addressBook = new AddressBook()); }
		}

		public Task<bool> RequestPermission()
		{
			return AddressBook.RequestPermission();
		}

		public IQueryable<Contact> Contacts
		{
			get { return AddressBook; }
		}

		public Contact LoadContact(string id)
		{
			return AddressBook.Load(id);
		}

		public bool LoadSupported
		{
			get { return true; }
		}

		public bool PreferContactAggregation
		{
			get;
			set;
		}

		public bool AggregateContactsSupported
		{
			get { return true; }
		}

		public bool SingleContactsSupported
		{
			get { return true; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}
	}
}

#endif