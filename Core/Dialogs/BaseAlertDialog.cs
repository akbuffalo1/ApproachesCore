using System;

namespace AD
{
	public class BaseAlertDialog : IAlertDialog
	{
		public AlertAlignment MessageAlignment { get; set; } = default(AlertAlignment);
		public string Title { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public Action Continuation { get; set; } = null;
	}
}