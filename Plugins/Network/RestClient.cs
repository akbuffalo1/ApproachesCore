#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.Net;
using AD.Exceptions;
using ModernHttpClient;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AD.Plugins.Network.Rest
{
    public class RestClient : IRestClient
    {
        private const string TAG = "AD.Network.RestClient";

        protected static async Task TryCatchAsync(Func<System.Threading.Tasks.Task> toTry, Action<Exception> errorAction)
        {
            try
            {
                await toTry();
            }
            catch (Exception exception)
            {
                errorAction(exception);
            }
        }

        protected static void TryCatch(Action toTry, Action<Exception> errorAction)
        {
            try
            {
                toTry();
            }
            catch (Exception exception)
            {
                errorAction(exception);
            }
        }

        protected Dictionary<string, object> Options { set; private get; }

        public RestClient()
        {
            Options = new Dictionary<string, object>
            {
                {KnownOptions.ForceWindowsPhoneToUseCompression, "true"}
            };
        }

        public void ClearSetting(string key)
        {
            try
            {
                Options.Remove(key);
            }
            catch (KeyNotFoundException)
            {
                // ignored - not a problem
            }
        }

        public void SetSetting(string key, object value)
        {
            Options[key] = value;
        }

        public IAbortable MakeRequest(RestRequest restRequest, Action<StreamRestResponse> successAction, Action<Exception> errorAction)
        {
            NativeMessageHandler httpHandler = null;
            HttpClient httpClient = null;
            CancellationTokenSource cancellationSource = new CancellationTokenSource();

            TryCatch(async () =>
                {
                    httpHandler = BuildHttpHandler(restRequest);
                    httpClient = BuildHttpClient(restRequest, httpHandler);

                    Action<HttpResponseMessage> processResponse = (response) => ProcessResponse(response, restRequest, httpClient, successAction, errorAction);
                    await ProcessRequestThan(restRequest, httpClient, cancellationSource.Token, processResponse, errorAction);
                }, errorAction);

            return httpClient != null ? new RestRequestAsyncHandle(httpClient, cancellationSource) : null;
        }

        public IAbortable MakeRequest(RestRequest restRequest, Action<RestResponse> successAction, Action<Exception> errorAction)
        {
            NativeMessageHandler httpHandler = null;
            HttpClient httpClient = null;
            CancellationTokenSource cancellationSource = new CancellationTokenSource();

            TryCatch(async () =>
                {
                    httpHandler = BuildHttpHandler(restRequest);
                    httpClient = BuildHttpClient(restRequest, httpHandler);

                    Action<HttpResponseMessage> processResponse = (response) => ProcessResponse(response, restRequest, httpClient, successAction, errorAction);
                    await ProcessRequestThan(restRequest, httpClient, cancellationSource.Token, processResponse, errorAction);
                }, errorAction);

            return httpClient != null ? new RestRequestAsyncHandle(httpClient, cancellationSource) : null;
        }

        protected virtual NativeMessageHandler BuildHttpHandler(RestRequest restRequest)
        {
            var httpHandler = new NativeMessageHandler();
            SetCookieContainer(restRequest, httpHandler);
            SetCredentials(restRequest, httpHandler);
            SetPlatformSpecificProperties(restRequest, httpHandler);
            return httpHandler;
        }

        protected virtual HttpClient BuildHttpClient(RestRequest restRequest, NativeMessageHandler httpHandler)
        {
            var httpClient = new HttpClient(httpHandler);
            SetAccept(restRequest, httpClient);
            SetUserAgent(restRequest, httpClient);
            SetCustomHeaders(restRequest, httpClient);
            return httpClient;
        }

        private static void SetCustomHeaders(RestRequest restRequest, HttpClient httpClient)
        {
            if (restRequest.Headers != null)
            {
                foreach (var kvp in restRequest.Headers)
                {
                    httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }
        }

        protected virtual void SetCredentials(RestRequest restRequest, NativeMessageHandler httpHandler)
        {
            if (restRequest.Credentials != null)
            {
                httpHandler.Credentials = restRequest.Credentials;
            }
        }

        protected virtual void SetCookieContainer(RestRequest restRequest, NativeMessageHandler httpHandler)
        {
            // note that we don't call
            //   httpRequest.SupportsCookieContainer
            // here - this is because Android complained about this...
            try
            {
                if (restRequest.CookieContainer != null)
                {
                    httpHandler.CookieContainer = restRequest.CookieContainer;
                }
            }
            catch (Exception exception)
            {
                AD.Resolver.Resolve<ILogger>().Warn(TAG, "Error masked during Rest call - cookie creation - {0}", exception.ToLongString());
            }
        }

        protected virtual void SetAccept(RestRequest restRequest, HttpClient httpClient)
        {
            if (!string.IsNullOrEmpty(restRequest.Accept))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(restRequest.Accept));
            }
        }

        protected virtual void SetUserAgent(RestRequest restRequest, HttpClient httpClient)
        {
            if (!string.IsNullOrEmpty(restRequest.UserAgent))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", restRequest.UserAgent);
            }
        }

        protected virtual void SetPlatformSpecificProperties(RestRequest restRequest, NativeMessageHandler httpRequest)
        {
            // do nothing by default
        }

        protected virtual void ProcessResponse(
            HttpResponseMessage response,
            RestRequest restRequest,
            HttpClient httpClient,
            Action<RestResponse> successAction,
            Action<Exception> errorAction)
        {
            var restResponse = new RestResponse
            {
                CookieCollection = restRequest.CookieContainer != null ? restRequest.CookieContainer.GetCookies(restRequest.Uri) : null,
                Tag = restRequest.Tag,
                StatusCode = response.StatusCode
            };
            successAction(restResponse);
        }

        protected virtual void ProcessResponse(
            HttpResponseMessage response,
            RestRequest restRequest,
            HttpClient httpRequest,
            Action<StreamRestResponse> successAction,
            Action<Exception> errorAction)
        {
            TryCatch(() =>
                {
                    using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                    {
                        var restResponse = new StreamRestResponse
                        {
                            CookieCollection = restRequest.CookieContainer != null ? restRequest.CookieContainer.GetCookies(restRequest.Uri) : null,
                            Stream = responseStream,
                            Tag = restRequest.Tag,
                            StatusCode = response.StatusCode
                        };
                        successAction(restResponse);
                    }
                }, errorAction);
        }

        protected virtual async Task ProcessRequestThan(
            RestRequest restRequest,
            HttpClient httpClient,
            CancellationToken cancelToken,
            Action<HttpResponseMessage> continueAction,
            Action<Exception> errorAction)
        {
            HttpResponseMessage response = null;

            await TryCatchAsync(async () =>
                {
                    switch (restRequest.Verb)
                    {
                        case Verbs.Get:
                            response = await httpClient.GetAsync(restRequest.Uri, cancelToken);
                            break;
                        case Verbs.Post:
                            response = await httpClient.PostAsync(restRequest.Uri, restRequest.Content, cancelToken);
                            break;
                        case Verbs.Patch:
                            response = await httpClient.PatchAsync(restRequest.Uri, restRequest.Content, cancelToken);
                            break;
                        case Verbs.Put:
                            response = await httpClient.PutAsync(restRequest.Uri, restRequest.Content, cancelToken);
                            break;
                        case Verbs.Delete:
                            response = await httpClient.DeleteAsync(restRequest.Uri, cancelToken);
                            break;
                    }

                    response.CustomEnsureSuccessStatusCode();

                    continueAction(response);
                }, errorAction);
        }
    }
}

public static class HttpResponseMessageExtensions
{
    public static void CustomEnsureSuccessStatusCode(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = response.Content.ReadAsStringAsync().Result;

        if (response.Content != null)
            response.Content.Dispose();

        throw new BetterHttpResponseException(response.StatusCode, content);
    }
}

public class BetterHttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public BetterHttpResponseException(HttpStatusCode statusCode, string content) : base(content)
    {
        StatusCode = statusCode;
    }
}

public static class HttpClientPatchExtension
{ 
    public static async Task<HttpResponseMessage> PatchAsync (this HttpClient client, Uri requestUri, HttpContent value,CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = value };
        try
        {
            var response = await client.SendAsync(request,cancellationToken);
            return response;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
#endif