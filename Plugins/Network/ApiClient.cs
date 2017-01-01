#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;

namespace AD.Plugins.Network.Rest
{
	public enum HttpProtocol
	{
		Http,
		Https
	};

	public enum Priority
	{
		Internet,
		Cache
	};

	public class UnavailableDataException : Exception
	{
		public UnavailableDataException() : base()
		{
		}
	}


	public class ApiClient : IApiClient
	{
		public IHttpServerConfig ServerConfig { get; protected set; }
		public IJsonConverter Serializer { get; protected set; }
		public IJsonRestClient RestClient { get; protected set; }
		public INetworkReachability NetworkReachability { get; protected set; }
		public IFileStore FileStore { get; protected set; }

		protected ILogger Logger { get; set; }

		private const string TAG = "AD.ApiClient";

		public ApiClient(
			ILogger logger,
			IHttpServerConfig config,
			IJsonConverter serializer,
			IJsonRestClient restClient,
			INetworkReachability networkReachability,
			IFileStore fileStore)
		{
			//DefaultRequestHeaders.Add("Accept", "application/json");
			this.ServerConfig = config;
			this.Serializer = serializer;
			this.RestClient = restClient;
			this.NetworkReachability = networkReachability;
			this.Logger = logger;
			this.FileStore = fileStore;
		}

		public virtual RestRequest CreateRequest(string path)
		{
			var request = new RestRequest(new Uri(path));
			return request;
		}

		public virtual RestRequest CreateRequest<T>(string path, T body) where T : class
		{
			var request = new JsonRestRequest<T>(new Uri(path)) { Body = body };
			return request;
		}

		void MakeInternetRequest<T>(string path, string slug, Action<T> successFunc, Action<Exception> errorAct, Action<string, T> storeData)//, Func<string, T> retrieveData)
		{
			var request = CreateRequest(ServerConfig.BaseAddress + path);
			RestClient.MakeRequestFor<T>(request, response =>
			{
				successFunc(response.Result);
				storeData(slug, response.Result);
			}, error =>
			{
				errorAct(error);
			});
		}

		public void MakeCacheableRequest<T>(Priority priority,
			string path,
			string slug,
			Action<T> successFunc,
			Action<Exception> errorAct,
			Action<string, T> storeData,
			Func<string, T> retrieveData)
		{
			Action<string> ReadFromCache = (fileName) =>
			{
				Logger.Debug(TAG, "Try cached data");
				var cachedData = retrieveData(fileName);
				if (cachedData == null)
				{
					Logger.Debug(TAG, "NO DATA");
					errorAct(new UnavailableDataException());
				}
				Logger.Debug(TAG, "Use Cache");
				successFunc(cachedData);
			};

			if (priority == Priority.Internet)
			{
				Logger.Debug(TAG, "Priority INET");
				if (NetworkReachability.IsConnected)
				{
					Logger.Debug(TAG, "Do Internet Request");
                    MakeInternetRequest<T>(path, slug, successFunc, (error) => {
                        if (!CheckIfUnauthoirized(error as BetterHttpResponseException))
                            ReadFromCache(slug);
                        else
                            errorAct(error);
                    } , storeData);
				}
				else
				{
					ReadFromCache(slug);
				}
			}
			else {  // priority cache
				Logger.Debug(TAG, "Priority CACHE");
				var cachedData = retrieveData(slug);
				if (cachedData == null)
				{
					Logger.Debug(TAG, "No cached data");
					if (NetworkReachability.IsConnected)
					{
						Logger.Debug(TAG, "Do Internet Request");
                        MakeInternetRequest<T>(path, slug, successFunc, (error) => {
                            errorAct(CheckIfUnauthoirized(error as BetterHttpResponseException) ? error : new UnavailableDataException());
                        }, storeData);
					}
					else {
						Logger.Debug(TAG, "NO DATA");
						errorAct(new UnavailableDataException());
					}
				}
				else {
					Logger.Debug(TAG, "Use Cache");
					successFunc(cachedData);
				}
			}
		}

		public void MakeFileCacheableRequest<T>(Priority priority,
			string path,
			string slug,
			Action<T> successAction,
			Action<Exception> errorAction)
		{

			if (slug == null)
				slug = GetSlug(path);

			Action<string, T> WriteFileAction = (fileName, result) =>
			{
				string data = Serializer.SerializeObject(result);
				FileStore.WriteFile(fileName, data);
			};

			Func<string, T> ReadDataFromFileFunc = delegate (string fileName)
			{
				string stringData;
				if (FileStore.TryReadTextFile(fileName, out stringData))
				{
					return Serializer.DeserializeObject<T>(stringData);
				}
				else
				{
					return default(T);
				}
			};

			MakeCacheableRequest(priority, path, slug, successAction, errorAction, WriteFileAction, ReadDataFromFileFunc);
		}

		private static string GetSlug(string path)
		{
			var strArray = path.Split('/');
			string fileName = "\\" + strArray[strArray.Length - 1];
			fileName += ".dat";
			return fileName;
		}

        private static bool CheckIfUnauthoirized(BetterHttpResponseException error) {
            return error != null && (error.StatusCode == System.Net.HttpStatusCode.Unauthorized || error.StatusCode == System.Net.HttpStatusCode.Forbidden);
        } 

		public IAbortable MakeRequest(string path, Action<RestResponse> successAction, Action<Exception> errorAction)
		{
			var request = CreateRequest(ServerConfig.BaseAddress + path);
			return RestClient.MakeRequest(request, successAction, errorAction);
		}

		public IAbortable MakeRequest(string path, Action<StreamRestResponse> successAction, Action<Exception> errorAction)
		{
			var request = CreateRequest(ServerConfig.BaseAddress + path);
			return RestClient.MakeRequest(request, successAction, errorAction);
		}

		public IAbortable MakeRequestFor<TResponse>(string path, Action<DecodedRestResponse<TResponse>> successAction, Action<Exception> errorAction, string verb = Verbs.Post)
		{
			var request = CreateRequest(ServerConfig.BaseAddress + path);
            request.Verb = verb;
			return RestClient.MakeRequestFor<TResponse>(request, successAction, errorAction);
		}

		public IAbortable MakeRequestFor<TResponse, TRequest>(string path, TRequest requestBody, Action<DecodedRestResponse<TResponse>> successAction, Action<Exception> errorAction, string verb = Verbs.Post) where TRequest : class
		{
			var request = CreateRequest<TRequest>(ServerConfig.BaseAddress + path, requestBody);
            request.Verb = verb;
			return RestClient.MakeRequestFor<TResponse>(request, successAction, errorAction);
		}

	}

}
#endif

