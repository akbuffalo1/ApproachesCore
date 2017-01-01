#if _FILE_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace AD
{
	/// <summary>
	/// Warning: this interface not implemented on all platforms currently
	/// </summary>
	public interface IFileStoreAsync
	{
		Task<AD.Plugins.File.TryResult<string>> TryReadTextFileAsync(string path);
		Task<AD.Plugins.File.TryResult<byte[]>> TryReadBinaryFileAsync(string path);
		Task<bool> TryReadBinaryFileAsync(string path, Func<Stream, Task<bool>> readMethod);
		Task WriteFileAsync(string path, string contents);
		Task WriteFileAsync(string path, IEnumerable<byte> contents);
		Task WriteFileAsync(string path, Func<Stream, Task> writeMethod);
	}
}
#endif