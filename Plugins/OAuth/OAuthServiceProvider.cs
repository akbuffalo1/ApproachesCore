#if _OAUTH_
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using AD.Plugins.Json;
using Newtonsoft.Json.Linq;
using TigerApp;
using Xamarin.Auth;

namespace AD.Plugins.OAuth
{
    public interface IOAuthServiceProvider
    {
        ISubject<bool> ConfigLoadedSubject { get; }
        OAuth1Authenticator ProvideAuthenticator();
        OAuth1Request ProvideAccountRequest(Account account);
        void Init(string providerName);
    }

    public class OAuthServiceProvider : IOAuthServiceProvider
    {
        readonly static string CONFIG_FILE_NAME = "oauthconfigmodel.json";

        public ISubject<bool> ConfigLoadedSubject { get; protected set; }

        private IJsonFileReader _oauthConfigReader;
        private JObject _configModel;
        private string _providerName;
		private OAuth1Authenticator _authenticator;

        public OAuthServiceProvider(IJsonFileReader oauthConfigReader)
        {
            _oauthConfigReader = oauthConfigReader;
            ConfigLoadedSubject = new Subject<bool>();
        }

        public void Init(string providerName) 
        {
            _providerName = providerName;
            _oauthConfigReader.LoadJson(CONFIG_FILE_NAME).SubscribeOnce(model =>
            {
                _configModel = model;
                ConfigLoadedSubject.OnNext(true);
            });
        }

		public OAuth1Authenticator ProvideAuthenticator()
		{
			return _authenticator ?? (_authenticator = _ProvideAuthenticator());
		}

		private OAuth1Authenticator _ProvideAuthenticator()
		{ 
			var configs = _configModel["oauth_versions"]["oauth1"]["providers"].ToObject<IEnumerable<JObject>>();
			if (null != configs)
			{
				var config = configs.FirstOrDefault(provider => ((string)provider["name"]).Equals(_providerName))["configs"]["auth"];
				return new OAuth1Authenticator(
					consumerKey: (string)config["consumerKey"],
					consumerSecret: (string)config["consumerSecret"],
					requestTokenUrl: new Uri((string)config["requestTokenUrl"]),
					authorizeUrl: new Uri((string)config["authorizeUrl"]),
					accessTokenUrl: new Uri((string)config["accessTokenUrl"]),
					callbackUrl: new Uri((string)config["callbackUrl"])
				);
			}
			return default(OAuth1Authenticator);
		}

		public OAuth1Request ProvideAccountRequest(Account account)
		{
			var config =
					_configModel["oauth_versions"]["oauth1"]["providers"]
					.ToObject<IEnumerable<JObject>>()
					.FirstOrDefault(provider => ((string)provider["name"]).Equals(_providerName))["configs"]["request"];

			return new OAuth1Request("GET", new Uri((string)config["verify_credentials_url"]), null, account);
		}
    }
}
#endif