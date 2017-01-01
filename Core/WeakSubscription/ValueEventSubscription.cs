namespace AD.WeakSubscription
{
    using System;
    using System.Reflection;

    public class ValueEventSubscription<T>
        : WeakEventSubscription<object, ValueEventArgs<T>>
    {
        public ValueEventSubscription(object source,
                                         EventInfo eventInfo,
                                         EventHandler<ValueEventArgs<T>> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<ValueEventArgs<T>>(this.OnSourceEvent);
        }
    }
}