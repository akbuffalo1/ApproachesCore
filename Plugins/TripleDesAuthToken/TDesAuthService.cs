#if _TDES_AUTH_TOKEN_

using System;
using AD.Plugins.DeviceInfo;
using AD.Plugins.Network.Rest;
using AD.Plugins.TripleDesAuthToken.RequestsEntities;

namespace AD.Plugins.TripleDesAuthToken
{
    public class TDesAuthService : ITDesAuthService
    {
        #region Fields

        private readonly ITDesAuthStore _authStore;
        private readonly ITDesAuthTokenServerConfig _serverConfig;
        private readonly IApiClient _apiClient;

        #endregion

        #region Constructors

        public TDesAuthService(ITDesAuthStore appConfig, ITDesAuthTokenServerConfig serverConfig, IApiClient apiClient)
        {
            _authStore = appConfig;
            _serverConfig = serverConfig;
            _apiClient = apiClient;
        }

        #endregion

        #region Properties

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetLoggedUsername());

        #endregion

        #region ITDesAuthService implementation

        public void UpdateDevice(Action<AuthData> successAction, Action<Exception> errorAction)
        {

            var authData = _authStore.GetAuthData();
            if (authData == null)
                authData = new AuthData()
                {
                    DeviceKey = string.Empty
                };
            var deviceKey = authData.DeviceKey;
            Resolver.Resolve<ILogger>().Debug("UpdateDevice", "DeviceKey: " + deviceKey);


            var devInfo = Resolver.Resolve<IDeviceInfo>();
            var payload = new DeviceAuthRequestEntity()
            {
                Device = $"{devInfo.Manufacturer}|{devInfo.Model}",
                OS = devInfo.Manufacturer.Equals("Apple") ? "iOS" : "Android",
                OSVersion = devInfo.SystemVersion
            };

            _apiClient.MakeRequestFor<DeviceAuthResponseEntity, DeviceAuthRequestEntity>(
                _serverConfig.DeviceAuthRequestPath,
                payload,
                res => OnDeviceTokenRequestSucceed(successAction, res),
                errorAction
            );
        }

        //TODO: Refactor CheckLogin call
        public void CheckLogin(string username, string password, Action<UserAuthResponseEntity> loginOK, Action<UserAuthResponseEntity> loginFail, Action<Exception> errorAction)
        {
            var authData = _authStore.GetAuthData();
            authData.Username = username;
            authData.Password = password;
            _authStore.SetAuthData(authData);

            _apiClient.MakeRequestFor<UserAuthResponseEntity>(
                _serverConfig.UserAuthRequestPath,
                res => loginOK(res.Result),
                error =>
                {
                    ClearUserData();
                    errorAction(error);
                });
        }

        public void Logout()
        {
            var authData = _authStore.GetAuthData();
            authData.Username = null;
            authData.Password = null;
            authData.AuthProvider = null;
            authData.AuthProviderToken = null;
            _authStore.SetAuthData(authData);
        }

        public string GetLoggedUsername()
        {
            return _authStore.GetAuthData()?.Username;
        }

        #endregion

        #region Private methods

        private void OnDeviceTokenRequestSucceed(Action<AuthData> successAction, DecodedRestResponse<DeviceAuthResponseEntity> res)
        {
            var authData = _authStore.GetAuthData();
            if (authData == null)
                authData = new AuthData();
            var deviceKey = authData.DeviceKey;
            Resolver.Resolve<ILogger>().Debug("UpdateDevice", "stored DeviceKey: " + deviceKey);
            authData.DeviceKey = res.Result.DeviceKey;
            Resolver.Resolve<ILogger>().Debug("UpdateDevice", "received DeviceKey: " + authData.DeviceKey);

            _authStore.SetAuthData(authData);
            successAction(authData);
        }

        // TODO: Better way to handle AuthData
        private void ClearUserData()
        {
            var authData = _authStore.GetAuthData();
            authData.Username = string.Empty;
            authData.Password = string.Empty;
            _authStore.SetAuthData(authData);
        }

        #endregion
    }
}

#endif