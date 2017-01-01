#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace AD.Plugins.Network.Rest
{
	public class RestRequest
	{
		public RestRequest(string url, string verb = Verbs.Get, string accept = ContentType.Json, string tag = null)
			: this(new Uri(url), verb, accept, tag)
		{
		}

		public RestRequest(Uri uri, string verb = Verbs.Get, string accept = ContentType.Json, string tag = null)
		{
			Uri = uri;
			Tag = tag;
			Verb = verb;
			Accept = accept;
			Headers = new Dictionary<string, string>();
			Options = new Dictionary<string, object>();
		}

		public string Tag { get; set; }
		public Uri Uri { get; set; }
		public string Verb { get; set; }
		public string ContentTypeString { get; set; }
		public string UserAgent { get; set; }
		public string Accept { get; set; }
		public Dictionary<string, string> Headers { get; set; }
		public CookieContainer CookieContainer { get; set; }
		public Dictionary<string, object> Options { get; set; }
		public ICredentials Credentials { get; set; }

		public virtual HttpContent Content
		{
			get { return null; }
		}
	}
}
#endif