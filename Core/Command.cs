using System;
using System.Windows.Input;

namespace AD.Core
{
	public class Command : ICommand
	{
		private readonly Func<bool> _canExecute;
		private readonly Action _execute;

		public Command(Action execute)
			: this(execute, null)
		{
		}

		public Command(Action execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		#region ICommand Members

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute();
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				_execute();
			}
		}

		public void Execute()
		{
			Execute(null);
		}

		#endregion

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
		    handler?.Invoke(this, EventArgs.Empty);
		}
	}

	public class Command<T> : ICommand
	{
		private readonly Func<T, bool> _canExecute;
		private readonly Action<T> _execute;

		public Command(Action<T> execute)
			: this(execute, null)
		{
		}

		public Command(Action<T> execute, Func<T, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		#region ICommand Members

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute((T)parameter);
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				_execute((T)parameter);
			}
		}

		public void Execute()
		{
			Execute(null);
		}

		#endregion

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
		    handler?.Invoke(this, EventArgs.Empty);
		}
	}
}

