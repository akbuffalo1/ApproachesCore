namespace AD.WeakSubscription
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
	using AD.Platform.Core;
	
    public class NamedNotifyPropertyChangedEventSubscription<T>
        : NotifyPropertyChangedEventSubscription
    {
        private readonly string _propertyName;

        public NamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              Expression<Func<T>> property,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : this(source, (string)source.GetPropertyNameFromExpression(property), targetEventHandler)
        {
        }

        public NamedNotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                              string propertyName,
                                                              EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, targetEventHandler)
        {
            this._propertyName = propertyName;
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler((sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.PropertyName)
                        || e.PropertyName == this._propertyName)
                    {
                        this.OnSourceEvent(sender, e);
                    }
                });
        }
    }
}