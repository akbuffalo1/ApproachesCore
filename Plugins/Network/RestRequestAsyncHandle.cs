#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System.Net;
using System.Net.Http;
using System.Threading;

namespace AD.Plugins.Network.Rest
{
	// Credit - this class heavily influenced by the wonderful https://github.com/restsharp/RestSharp
	//        - credit here under Apache 2.0 license
	public class RestRequestAsyncHandle
		: IAbortable
    
	{
		private readonly HttpClient _httpClient;
		private CancellationTokenSource _cancel;

		public RestRequestAsyncHandle(HttpClient httpClient, CancellationTokenSource cancel)
		{
			_httpClient = httpClient;
			_cancel = cancel;
		}

		public void Abort()
		{
			if (_httpClient != null)
				_httpClient.CancelPendingRequests();

			if (_cancel != null)
				_cancel.Cancel();
		}
	}
}
#endif
