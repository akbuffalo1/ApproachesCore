#if __ANDROID__ && _CONTACTS_

using System;
using System.Linq;
using System.Threading.Tasks;
using AD.Plugins.Permissions;

namespace AD.Plugins.Contacts.Droid
{
	/// <summary>
	/// Implementation for Feature
	/// </summary>
	public class ContactsDroid : IContacts
	{
		public async Task<bool> RequestPermission()
		{
			var permissionsPlugin = Resolver.Resolve<IPermissions>();
			var status = await permissionsPlugin.CheckPermissionStatusAsync(Permission.Contacts).ConfigureAwait(false);
			if (status != PermissionStatus.Granted)
			{
				Console.WriteLine("Currently does not have Contacts permissions, requesting permissions");

				var request = await permissionsPlugin.RequestPermissionsAsync(Permission.Contacts);

				if (request[Permission.Contacts] != PermissionStatus.Granted)
				{
					Console.WriteLine("Contacts permission denied, can not get positions async.");
					return false;
				}
			}

			return true;
		}

		private AddressBook addressBook;
		public IQueryable<Contact> Contacts
		{
			get
			{
				return (IQueryable<Contact>)AddressBook;
			}
		}
		private AddressBook AddressBook
		{
			get
			{
				return addressBook ?? (addressBook = new AddressBook(Android.App.Application.Context));
			}
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
			get
			{
				return AddressBook.PreferContactAggregation;
			}
			set
			{
				AddressBook.PreferContactAggregation = value;
			}
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