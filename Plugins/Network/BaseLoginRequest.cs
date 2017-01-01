#if _NETWORK_ || _TDES_AUTH_TOKEN_
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Network
{
    public class BaseLoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
#endif