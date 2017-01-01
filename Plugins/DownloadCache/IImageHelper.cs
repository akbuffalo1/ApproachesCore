#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public interface IImageHelper<T>
        : IDisposable
        where T : class
    {
        string DefaultImagePath { get; set; }
        string ErrorImagePath { get; set; }
        string ImageUrl { get; set; }
        event EventHandler<ValueEventArgs<T>> ImageChanged;
        int MaxWidth { get; set; }
        int MaxHeight { get; set; }
    }
}
#endif