#if _REACTIVEUI_
using System;
using System.Reactive.Linq;

namespace AD.Core.Reactive
{
    public static class ReactiveEx
    {
        public static void DisposeWith(this IDisposable self, Action<IDisposable> ToDispose)
        {
            ToDispose(self);
        }

        public static IObservable<TResult> NotNull<TResult>(this IObservable<TResult> self)
        where TResult : class
        {
            return self.Where(_ => _ != null);
        }

        public static void SubscribeOnce<T>(this IObservable<T> observable, IObserver<T> onNext)
        {
            IDisposable disposable = null;
            disposable = observable.Subscribe(t =>
            {
                onNext.OnNext(t);
                onNext.OnCompleted();
                disposable?.Dispose();
                disposable = null;
            }, onNext.OnError);
        }

        public static void SubscribeOnce<T>(this IObservable<T> observable, Action<T> onNext)
        {
            IDisposable disposable = null;
            disposable = observable.Subscribe(t =>
            {
                onNext(t);
                disposable?.Dispose();
                disposable = null;
            });
        }

        public static IObservable<T> ObserveOnUI<T>(this IObservable<T> observable)
        {
            return observable.ObserveOn(ReactiveUI.RxApp.MainThreadScheduler);
        }

        public static IObservable<T> Catch<T>(this IObservable<T> observable, Action<Exception> errorHandler)
        {
            return observable.Catch<T, Exception>((ex) =>
            {
                errorHandler(ex);
                return Observable.Empty<T>();
            });
        }
    }
}
#endif
