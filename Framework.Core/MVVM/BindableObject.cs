namespace Framework.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A simple implementation of the INotifyPropertyChanged.
    /// </summary>

    public class BindableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs after a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs before a property value changes.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Extracts the name of a property from an expression.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:01 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     The type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     An expression returning the property's name.
        /// </param>
        ///
        /// <returns>
        ///     The name of the property returned by the expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return member.Name;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Raises the PropertyChanged event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:01 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     The type of the property that changed.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     An expression identifying the property that changed.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = GetPropertyName<T>(propertyExpression);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Raises the PropertyChanged event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     If the propertyName parameter does not correspond to an existing property on the current
        ///     class, an exception is thrown in DEBUG configuration only.
        /// </remarks>
        ///
        /// <param name="propertyName">
        ///     The name of the property that changed.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Raises the PropertyChanging event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     If the propertyName parameter does not correspond to an existing property on the current
        ///     class, an exception is thrown in DEBUG configuration only.
        /// </remarks>
        ///
        /// <param name="propertyName">
        ///     The name of the property that changed.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void RaisePropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
            if (propertyChanging != null)
            {
                propertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Raises the PropertyChanging event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:01 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     The type of the property that changes.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     An expression identifying the property that changes.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
            if (propertyChanging != null)
            {
                string propertyName = GetPropertyName<T>(propertyExpression);
                propertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Assigns a new value to the property. Then, raises the PropertyChanged event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:01 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     The type of the property that changed.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     An expression identifying the property that changed.
        /// </param>
        /// <param name="field">
        ///     [in,out] The field storing the property's value.
        /// </param>
        /// <param name="newValue">
        ///     The property's value after the change occurred.
        /// </param>
        ///
        /// <returns>
        ///     True if the PropertyChanged event has been raised, false otherwise. The event is not
        ///     raised if the old value is equal to the new value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            this.RaisePropertyChanging<T>(propertyExpression);
            field = newValue;
            this.RaisePropertyChanged<T>(propertyExpression);
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Assigns a new value to the property. Then, raises the PropertyChanged event if needed.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:01 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     The type of the property that changed.
        /// </typeparam>
        /// <param name="propertyName">
        ///     The name of the property that changed.
        /// </param>
        /// <param name="field">
        ///     [in,out] The field storing the property's value.
        /// </param>
        /// <param name="newValue">
        ///     The property's value after the change occurred.
        /// </param>
        ///
        /// <returns>
        ///     True if the PropertyChanged event has been raised, false otherwise. The event is not
        ///     raised if the old value is equal to the new value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected bool Set<T>(string propertyName, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            this.RaisePropertyChanging(propertyName);
            field = newValue;
            this.RaisePropertyChanged(propertyName);
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
        ///-------------------------------------------------------------------------------------------------
        protected void Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            this.Set(propertyName, ref field, newValue);
        }

        /// <summary>
        /// Provides access to the PropertyChanged event handler to derived classes.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return this.PropertyChanged;
            }
        }

        /// <summary>
        /// Provides access to the PropertyChanging event handler to derived classes.
        /// </summary>
        protected PropertyChangingEventHandler PropertyChangingHandler
        {
            get
            {
                return this.PropertyChanging;
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
        /// be broadcasted. If false, only the event will be raised.</param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue)
        {
            PropertyChangedEventHandler propertyChangedHandler = this.PropertyChangedHandler;
            if ((propertyChangedHandler != null))
            {
                string propertyName = BindableObject.GetPropertyName<T>(propertyExpression);
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
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
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue = default(T), T newValue = default(T))
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            }
           
            this.RaisePropertyChanged(propertyName);
        }


    }
}
