#if _OAUTH_
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace AD.Plugins.OAuth
{
    public interface IOAuthAccountHelper
    {
        void Save(Account account, string providerName);
        void TrySaveIfNotNull(Account account, string providerName);
		void Delete(Account account, string providerName);
        void TryDeleteIfNotNullAndExist(Account account, string providerName);
        IObservable<IEnumerable<Account>> FindAccountsObservable(string providerName);
        Task<IEnumerable<Account>> FindAccountsAsync(string providerName);
		void StartAuthorization(OAuth1Authenticator authenetificator);
    }
}
#endif
