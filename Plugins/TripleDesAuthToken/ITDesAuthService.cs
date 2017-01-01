#if _TDES_AUTH_TOKEN_

using System;
using AD.Plugins.TripleDesAuthToken.RequestsEntities;
using AD.Plugins.TripleDesAuthToken;

namespace AD
{
	public interface ITDesAuthService
	{
		void UpdateDevice(Action<AuthData> successAction, Action<Exception> errorAction);

		void CheckLogin(string username, string password,
						 Action<UserAuthResponseEntity> loginOKAction, Action<UserAuthResponseEntity> loginKOAction, Action<Exception> errorAction);

		string GetLoggedUsername();

		bool IsLoggedIn { get; }

		void Logout();
	}
}

#endif
