#if __ANDROID__
using System;
using Android.OS;
using Android.App;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Support.V7.App;

namespace AD.Droid
{
	public class ActivityBase<TSetup, TViewModel> : AppCompatActivity
		where TSetup : AD.ApplicationSetupBase, new()
		where TViewModel : AD.ViewModelBase
	{
		protected TViewModel ViewModel { private set; get; }
		protected List<IDisposable> Disposables { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			AD.ApplicationBase<TSetup>.CheckInitialized();
			Disposables = new List<IDisposable>();

			ViewModel = Resolver.Resolve<TViewModel>();

			Resolver.Resolve<IDependencyContainer>().Register<IAndroidGlobals>(new AndroidGlobals(this.ApplicationContext));
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ViewModel.Dispose();
			ViewModel.ClearSubscriptions();
			Disposables.ForEach(d => d.Dispose());
		}

		protected override void OnResume()
		{
			base.OnResume();
			Subscribe();
		}

		protected override void OnPause()
		{
			base.OnPause();
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
	}
}
#endif

