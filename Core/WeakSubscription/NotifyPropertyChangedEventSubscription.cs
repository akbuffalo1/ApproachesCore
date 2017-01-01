namespace AD.WeakSubscription
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class NotifyPropertyChangedEventSubscription
        : WeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private static readonly EventInfo PropertyChangedEventInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged");

        // This code ensures the PropertyChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
        {
            iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
        }

        public NotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                         EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, PropertyChangedEventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(this.OnSourceEvent);
        }
    }
}