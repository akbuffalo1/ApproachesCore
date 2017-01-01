#if _FILE_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.IO;

namespace AD
{
	public interface IFileStore
	{
		bool TryReadTextFile(string path, out string contents);
		bool TryReadBinaryFile(string path, out Byte[] contents);
		bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);
		void WriteFile(string path, string contents);
		void WriteFile(string path, IEnumerable<Byte> contents);
		void WriteFile(string path, Action<Stream> writeMethod);
		bool TryMove(string from, string to, bool deleteExistingTo);
		bool Exists(string path);
		bool FolderExists(string folderPath);
		string PathCombine(string items0, string items1);
		string NativePath(string path);
		string FullPath(string path);

		void EnsureFolderExists(string folderPath);
		IEnumerable<string> GetFilesIn(string folderPath);
		void DeleteFile(string path);
		void DeleteFolder(string folderPath, bool recursive);

		Stream OpenRead(string path);
		Stream OpenWrite(string path);
		Stream OpenReadFromAssets (string path);
	}
}
#endif