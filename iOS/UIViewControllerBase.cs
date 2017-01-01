#if __IOS__
namespace AD.iOS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using UIKit;
	using Foundation;

	public class UIViewControllerBase<TSetup, TViewModel> : UIViewController
		where TSetup : AD.ApplicationSetupBase, new()
		where TViewModel : AD.ViewModelBase
	{
		protected TViewModel ViewModel { private set; get; }

		[Obsolete("ViewModel add to its own disposable list automatically")]
		protected List<IDisposable> Disposables { get; private set; }

		public UIViewControllerBase() : base() { }
		public UIViewControllerBase(string nib, NSBundle bundle) : base(nib, bundle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ApplicationBase<TSetup>.CheckInitialized();
			Disposables = new List<IDisposable>();

			ViewModel = Resolver.Resolve<TViewModel>();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Subscribe();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			Unsubscribe();
		}

		/// <summary>
		/// Do all subscriptions here(don't forget to call base)
		/// </summary>
		protected virtual void Subscribe()
		{
		}

		/// <summary>
		/// Do all UnSubscriptions here(don't forget to call base)
		/// </summary>
		protected virtual void Unsubscribe()
		{
			ViewModel.ClearSubscriptions();
		}

		//NOTE: this if for backward compatibility
		~UIViewControllerBase()
		{
			ViewModel.Dispose();
			ViewModel.ClearSubscriptions(); //just in case if somebody overriden Unsubscribe method
			Disposables.ForEach(disposable => disposable.Dispose());
		}
	}
}
#endif
