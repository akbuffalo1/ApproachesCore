#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public class FileDownloadRequest
    {
        public FileDownloadRequest(string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;
        }

        public string DownloadPath { get; private set; }
        public string Url { get; private set; }

        public event EventHandler<FileDownloadedEventArgs> DownloadComplete;

        public event EventHandler<ValueEventArgs<Exception>> DownloadFailed;

        public void Start()
        {
            try
            {
                var request = WebRequest.Create(new Uri(Url));
                request.BeginGetResponse((result) => ProcessResponse(request, result), null);
            }
            catch (Exception e)
            {
                FireDownloadFailed(e);
            }
        }

        private void ProcessResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                var fileService = FileStoreHelper.SafeGetFileStore();
                var tempFilePath = DownloadPath + ".tmp";

                using (var resp = request.EndGetResponse(result))
                {
                    using (var s = resp.GetResponseStream())
                    {
                        fileService.WriteFile(tempFilePath,
                                              fileStream =>
                                              {
                                                  var buffer = new byte[4 * 1024];
                                                  int count;
                                                  while ((count = s.Read(buffer, 0, buffer.Length)) > 0)
                                                  {
                                                      fileStream.Write(buffer, 0, count);
                                                  }
                                              });
                    }
                }
                fileService.TryMove(tempFilePath, DownloadPath, true);
            }
            catch (Exception exception)
            {
                FireDownloadFailed(exception);
                return;
            }

            FireDownloadComplete();
        }

        private void FireDownloadFailed(Exception exception)
        {
            var handler = DownloadFailed;
            handler?.Invoke(this, new ValueEventArgs<Exception>(exception));
        }

        private void FireDownloadComplete()
        {
            var handler = DownloadComplete;
            handler?.Invoke(this, new FileDownloadedEventArgs(Url, DownloadPath));
        }
    }
}
#endif