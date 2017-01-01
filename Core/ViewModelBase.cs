using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace AD
{
	public class ViewModelBase : IDisposable
	{
		private class ObservableAttribute<T> : ISubject<T>, IDisposable, ISubject<T, T>, IObserver<T>, IObservable<T>
		{
			private ReplaySubject<T> _subject;
			private WeakReference _viewModel;

			internal ObservableAttribute(ViewModelBase viewModel) : base()
			{
				_viewModel = new WeakReference(viewModel);
				_subject = new ReplaySubject<T>(0);
			}

			#region IObservable implementation

			public IDisposable Subscribe(IObserver<T> observer)
			{
				var disposable = _subject.Subscribe(observer);
				(_viewModel.Target as ViewModelBase).AddToDisposables(disposable); //NOTE: for backward compatibility
				(_viewModel.Target as ViewModelBase).Subscriptions.Add(disposable);
				return disposable;
			}

			#endregion

			#region IObserver implementation

			public void OnCompleted()
			{
				_subject.OnCompleted();
			}

			public void OnError(Exception error)
			{
				_subject.OnError(error);
			}

			public void OnNext(T value)
			{
				_subject.OnNext(value);
			}

			#endregion

			#region IDisposable implementation

			public void Dispose()
			{
				_subject.Dispose();
			}

			#endregion
		}

		private List<IDisposable> Subscriptions { get; set; }

		[Obsolete("Do not use it, disposables now added and processed automatically on subscribe")]
		protected List<IDisposable> Disposables { get; set; }

		public ISubject<string> GenericError { get; set; }
		public ViewModelBase()
		{
			Disposables = new List<IDisposable>();
			Subscriptions = new List<IDisposable>();
			GenericError = new ObservableAttribute<string>(this);
		}

		protected ISubject<T> CreateSubject<T>(Action<T> onNext = null)
		{
			var subject = new ObservableAttribute<T>(this);
			if (onNext != null)
				subject.Subscribe(onNext);
			return subject;
		}

		#region Public methods

		public virtual void Init()
		{
		}

		public void ClearSubscriptions()
		{
			Subscriptions.ForEach(subscr => subscr.Dispose());
			Subscriptions.Clear();
		}

		public void Dispose()
		{
			Disposables.ForEach(disposable =>
			{
				disposable.Dispose();
			});
		}

		public void DisplayConfirmation(string title, string message, Action continuation)
		{
			Resolver.Resolve<IDialogProvider>().DisplayConfirmation(title, message, continuation);
		}

		public void DisplayAlertDialog(IAlertDialog dlg)
		{
			Resolver.Resolve<IDialogProvider>().DisplayAlertDialog(dlg);
		}

		public void DisplayError(string message)
		{
			GenericError.OnNext(message);
			Resolver.Resolve<IDialogProvider>().DisplayError(message);
		}

		public void DisplayError(Exception error)
        {
            GenericError.OnNext(error.Message);
            Resolver.Resolve<IDialogProvider>().DisplayError(error.Message);
		}

		#endregion

		[Obsolete("Do not use it by yourself as it is already done by subscribe method automatically")]
		public void AddToDisposables(IDisposable disposable)
		{
			Disposables.Add(disposable);
		}
	}
}

