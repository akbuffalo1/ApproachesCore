namespace AD.WeakSubscription
{
    using System;
    using System.Reflection;

    public class GeneralEventSubscription
        : WeakEventSubscription<object, EventArgs>
    {
        public GeneralEventSubscription(object source,
                                           EventInfo eventInfo,
                                           EventHandler<EventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(this.OnSourceEvent);
        }
    }

    /*
    public class MvxGeneralEventSubscription<TSource, TEventArgs>
        : MvxWeakEventSubscription<TSource, TEventArgs>
        where TSource : class
        where TEventArgs : EventArgs
    {
        public MvxGeneralEventSubscription(TSource source,
                                           EventInfo eventInfo,
                                           EventHandler<TEventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        public MvxGeneralEventSubscription(TSource source,
                                           string eventName,
                                           EventHandler<TEventArgs> eventHandler)
            : base(source, typeof(TSource).GetEvent(eventName), eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }
    }
     */
}