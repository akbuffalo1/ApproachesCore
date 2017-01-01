#if _TDES_AUTH_TOKEN_

using System;

namespace AD.Plugins.TripleDesAuthToken
{
	public class TDesAuthStore : ITDesAuthStore
	{
		public string AuthToken { get; set; }

		public event EventHandler<AuthData> OnAuthDataChanged;

		public AuthData GetAuthData()
		{
			if (this.AuthToken == null)
			{
				return null;
			}
			var encryption = AD.Resolver.Resolve<Encryption.IEncryptionService>();
			var jsonData = encryption.DecryptData(this.AuthToken);
			var serializer = AD.Resolver.Resolve<IJsonConverter>();
			return serializer.DeserializeObject<AuthData>(jsonData);
		}

		public void SetAuthData(AuthData data)
		{
			var encryption = AD.Resolver.Resolve<Encryption.IEncryptionService>();
			var serializer = AD.Resolver.Resolve<IJsonConverter>();
			this.AuthToken = encryption.EncryptData(serializer.SerializeObject(data));
			this.Store();
			OnAuthDataChanged?.Invoke(this, data);
		}

		protected void Store()
		{
			var fileStore = AD.Resolver.Resolve<IFileStore>();
			var serializer = AD.Resolver.Resolve<IJsonConverter>();
			var encryption = AD.Resolver.Resolve<Encryption.IEncryptionService>();

			fileStore.WriteFile(AD.Resolver.Resolve<ITDesAuthConfig>().ConfigFile, AuthToken);
		}
	}
}

#endif
