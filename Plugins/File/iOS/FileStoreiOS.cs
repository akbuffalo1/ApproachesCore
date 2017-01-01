#if __IOS__ && (_FILE_ || _TDES_AUTH_TOKEN_)
using System;
using System.IO;

namespace AD.Plugins.File.iOS
{
	public class FileStoreiOS : AD.Plugins.File.FileStore
	{
		public FileStoreiOS ()
		{

		}
		public const string ResScheme = "res:";

		public override string FullPath (string path)
		{
			if (path.StartsWith (ResScheme))
				return path.Substring (ResScheme.Length);

			return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), path);
		}

		public override Stream OpenReadFromAssets (string path)
		{
			return null;
			//return Context.Assets.Open (path);
		}
	}
}

#endif