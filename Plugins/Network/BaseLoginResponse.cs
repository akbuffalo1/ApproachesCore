#if _NETWORK_ || _TDES_AUTH_TOKEN_
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Network
{
    public class BaseLoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
#endif