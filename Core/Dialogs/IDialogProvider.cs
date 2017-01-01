using System;

namespace AD
{
	public interface IDialogProvider
	{
		void DisplayAlertDialog(IAlertDialog dlg);
		void DisplayError(string message);
		void DisplayConfirmation(string title, string message, Action continuation);
	}
}
