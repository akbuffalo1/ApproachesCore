#if __ANDROID__ && (_FILE_ || _TDES_AUTH_TOKEN_)
using System;
using System.IO;
using Android.Content;

namespace AD.Plugins.File.Droid
{
	public class FileStoreDroid
		: AD.Plugins.File.FileStore
	{
		private Context _context;

		private Context Context
		{
			get
			{
				if (_context == null)
				{
					_context = Android.App.Application.Context;
				}
				return _context;
			}
		}

		public override string FullPath(string path)
		{
			return Path.Combine(Context.FilesDir.Path, path);
		}

		public override Stream OpenReadFromAssets (string path)
		{
			return Context.Assets.Open (path);
		}
	}
}
#endif
