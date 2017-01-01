namespace AD.WeakSubscription
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Input;

    public static class WeakSubscriptionExtensionMethods
    {
        public static NotifyPropertyChangedEventSubscription WeakSubscribe(this INotifyPropertyChanged source,
                                                                              EventHandler<PropertyChangedEventArgs>
                                                                                  eventHandler)
        {
            return new NotifyPropertyChangedEventSubscription(source, eventHandler);
        }

        public static NamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               Expression<Func<T>> property,
                                                                               EventHandler<PropertyChangedEventArgs>
                                                                                   eventHandler)
        {
            return new NamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static NamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               string property,
                                                                               EventHandler<PropertyChangedEventArgs>
                                                                                   eventHandler)
        {
            return new NamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static NotifyCollectionChangedEventSubscription WeakSubscribe(this INotifyCollectionChanged source,
                                                                                EventHandler
                                                                                    <NotifyCollectionChangedEventArgs>
                                                                                    eventHandler)
        {
            return new NotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        public static GeneralEventSubscription WeakSubscribe(this EventInfo eventInfo,
                                                                object source,
                                                                EventHandler<EventArgs> eventHandler)
        {
            return new GeneralEventSubscription(source, eventInfo, eventHandler);
        }

        public static ValueEventSubscription<T> WeakSubscribe<T>(this EventInfo eventInfo,
                                                                    object source,
                                                                    EventHandler<ValueEventArgs<T>> eventHandler)
        {
            return new ValueEventSubscription<T>(source, eventInfo, eventHandler);
        }

        public static CanExecuteChangedEventSubscription WeakSubscribe(this ICommand source,
                                                                          EventHandler<EventArgs> eventHandler)
        {
            return new CanExecuteChangedEventSubscription(source, eventHandler);
        }
    }
}