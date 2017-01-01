#if _SMS_
using System;
namespace AD
{
	public interface ISmsTask
	{
		void SendTo(string phoneNumber, string body);
	}
}
#endif