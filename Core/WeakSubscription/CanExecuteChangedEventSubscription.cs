namespace AD.WeakSubscription
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    public class CanExecuteChangedEventSubscription
        : WeakEventSubscription<ICommand, EventArgs>
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged");

        public CanExecuteChangedEventSubscription(ICommand source,
                                                    EventHandler<EventArgs> eventHandler)
            : base(source, CanExecuteChangedEventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(this.OnSourceEvent);
        }
    }
}