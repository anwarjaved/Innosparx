namespace Framework.Wpf.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Framework.MVVM;
    using Framework.Notification;

    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public abstract class ViewModelBase : BindableObject
    {
        private static bool? isInDesignMode;

        private readonly INotification notification;

        private readonly string topicID;
        protected ViewModelBase()
        {
            this.notification = Framework.Ioc.Container.TryGet<INotification>();

            if (this.Notification != null)
            {
                this.topicID = this.Notification.CreateTopic(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue, bool broadcast)
        {
            PropertyChangedEventHandler propertyChangedHandler = base.PropertyChangedHandler;
            if ((propertyChangedHandler != null) || broadcast)
            {
                string propertyName = BindableObject.GetPropertyName<T>(propertyExpression);
                if (propertyChangedHandler != null)
                {
                    propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
                }

                if (broadcast)
                {
                    this.Broadcast<T>(oldValue, newValue, propertyName);
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue = default(T), T newValue = default(T), bool broadcast = false)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            }
            this.RaisePropertyChanged(propertyName);

            if (broadcast)
            {
                this.Broadcast<T>(oldValue, newValue, propertyName);
            }
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available). 
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        /// <returns>True if the PropertyChanged event was raised, false otherwise.</returns>
        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue, bool broadcast)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            this.RaisePropertyChanging<T>(propertyExpression);
            T oldValue = field;
            field = newValue;
            this.RaisePropertyChanged<T>(propertyExpression, oldValue, field, broadcast);
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available). 
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        /// <returns>True if the PropertyChanged event was raised, false otherwise.</returns>
        protected bool Set<T>(string propertyName, ref T field, T newValue = default(T), bool broadcast = false)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            this.RaisePropertyChanging(propertyName);
            T oldValue = field;
            field = newValue;
            this.RaisePropertyChanged<T>(propertyName, oldValue, field, broadcast);
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Assigns a new value to the property. Then, raises the PropertyChanged event if needed,
        ///     and broadcasts a PropertyChangedMessage using the Messenger instance (or the static
        ///     default instance if no Messenger instance is available).
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/01/2014 11:49 AM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="field">
        ///     [in,out] The field storing the property's value.
        /// </param>
        /// <param name="newValue">
        ///     The property's value after the change occurred.
        /// </param>
        /// <param name="propertyName">
        ///     (Optional) The name of the property that changed.
        /// </param>
        /// <param name="broadcast">
        ///     (Optional) If true, a PropertyChangedMessage will be broadcasted. If false, only the
        ///     event will be raised.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected void Set<T>(ref T field, T newValue, bool broadcast = false, [CallerMemberName] string propertyName = "")
        {
            this.Set(propertyName, ref field, newValue, broadcast);
        }
        
        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running under Blend or Visual Studio).
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
                    isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                }
                return isInDesignMode.Value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the topic identifier of this model.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/01/2014 2:13 PM.
        /// </remarks>
        ///
        /// <value>
        ///     The topic identifier of this model.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string TopicID
        {
            get
            {
                return this.topicID;
            }
        }

        public INotification Notification
        {
            get
            {
                return this.notification;
            }
        }

        /// <summary>
        /// Broadcasts a PropertyChangedMessage using either the instance of
        /// the Messenger that was passed to this class (if available) 
        /// or the Messenger's default instance.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="oldValue">The value of the property before it
        /// changed.</param>
        /// <param name="newValue">The value of the property after it
        /// changed.</param>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            if (this.Notification != null)
            {
                PropertyChangedMessage<T> message = new PropertyChangedMessage<T>(this, oldValue, newValue, propertyName);
                this.Notification.Publish<PropertyChangedMessage<T>>(this.TopicID, message);

            }
        }
    }
}
