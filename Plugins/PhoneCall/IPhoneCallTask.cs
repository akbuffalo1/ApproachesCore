#if _PHONE_CALL_
using System;
namespace AD
{
	public interface IPhoneCallTask
	{
		void MakePhoneCall(string name, string number);
	}
}
#endif