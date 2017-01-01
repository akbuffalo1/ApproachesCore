#if _TDES_AUTH_TOKEN_

using System;
using AD.Plugins.TripleDesAuthToken;

namespace AD
{
	public interface ITDesAuthStore
	{
		string AuthToken { get; set; }
		//string DeviceToken { get; set; }
		event EventHandler<AuthData> OnAuthDataChanged;

		AuthData GetAuthData ();
		void SetAuthData (AuthData data); // update token
		//void SetAuthTokenFromData<T>(T data); 
	}
}

#endif