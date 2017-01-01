#if _MAIL_
using System;
using System.IO;

namespace AD.Plugins.Email
{
	public class EmailAttachment
	{
		public string ContentType { get; set; }
		public string FileName { get; set; }
		public Stream Content { get; set; }
	}
}
#endif