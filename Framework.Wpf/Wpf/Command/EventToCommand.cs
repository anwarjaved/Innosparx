namespace Framework.Wpf.Command
{
    using System;
    using System.Security;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// This <see cref="T:System.Windows.Interactivity.TriggerAction`1" /> can be
    /// used to bind any event on any FrameworkElement to an <see cref="T:System.Windows.Input.ICommand" />.
    /// Typically, this element is used in XAML to connect the attached element
    /// to a command located in a ViewModel. This trigger can only be attached
    /// to a FrameworkElement or a class deriving from FrameworkElement.
    /// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
    /// and leave the CommandParameter and CommandParameterValue empty!</para>
    /// </summary>
    
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        private object commandParameterValue;
        private bool? mustToggleValue;

        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty;
        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty;
        /// <summary>
        /// Identifies the <see cref="EventArgsConverterParameter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventArgsConverterParameterProperty;
        /// <summary>
        /// The <see cref="EventArgsConverterParameter" /> dependency property's name.
        /// </summary>
        public const string EventArgsConverterParameterPropertyName = "EventArgsConverterParameter";
        /// <summary>
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty;

        static EventToCommand()
        {
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
            {
                EventToCommand command = s as EventToCommand;
                if ((command != null) && (command.AssociatedObject != null))
                {
                    command.EnableDisableElement();
                }
            }));
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, (s, e) => OnCommandChanged(s as EventToCommand, e)));
            MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventToCommand), new PropertyMetadata(false, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
            {
                EventToCommand command = s as EventToCommand;
                if ((command != null) && (command.AssociatedObject != null))
                {
                    command.EnableDisableElement();
                }
            }));
            EventArgsConverterParameterProperty = DependencyProperty.Register("EventArgsConverterParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null));
        }

        private bool AssociatedElementIsDisabled()
        {
            FrameworkElement associatedObject = this.GetAssociatedObject();
            return ((base.AssociatedObject == null) || ((associatedObject != null) && !associatedObject.IsEnabled));
        }

        private void EnableDisableElement()
        {
            FrameworkElement associatedObject = this.GetAssociatedObject();
            if (associatedObject != null)
            {
                ICommand command = this.GetCommand();
                if (this.MustToggleIsEnabledValue && (command != null))
                {
                    associatedObject.IsEnabled = command.CanExecute(this.CommandParameterValue);
                }
            }
        }

        /// <summary>
        /// This method is here for compatibility
        /// with the Silverlight version.
        /// </summary>
        /// <returns>The FrameworkElement to which this trigger
        /// is attached.</returns>
        private FrameworkElement GetAssociatedObject()
        {
            return (base.AssociatedObject as FrameworkElement);
        }

        /// <summary>
        /// This method is here for compatibility
        /// with the Silverlight 3 version.
        /// </summary>
        /// <returns>The command that must be executed when
        /// this trigger is invoked.</returns>
        private ICommand GetCommand()
        {
            return this.Command;
        }

        /// <summary>
        /// Provides a simple way to invoke this trigger programatically
        /// without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            this.Invoke(null);
        }

        /// <summary>
        /// Executes the trigger.
        /// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
        /// and leave the CommandParameter and CommandParameterValue empty!</para>
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        
        protected override void Invoke(object parameter)
        {
            if (!this.AssociatedElementIsDisabled())
            {
                ICommand command = this.GetCommand();
                object parameterValue = this.CommandParameterValue;

                if ((parameterValue == null) && this.PassEventArgsToCommand)
                {
                    parameterValue = (this.EventArgsConverter == null) ? parameter : this.EventArgsConverter.Convert(parameter, this.EventArgsConverterParameter);
                }
                if ((command != null) && command.CanExecute(parameterValue))
                {
                    command.Execute(parameterValue);
                }
            }
        }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        
        protected override void OnAttached()
        {
            base.OnAttached();
            this.EnableDisableElement();
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.EnableDisableElement();
        }

        private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element != null)
            {
                if (e.OldValue != null)
                {
                    ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
                }
                ICommand newValue = (ICommand)e.NewValue;
                if (newValue != null)
                {
                    newValue.CanExecuteChanged += element.OnCommandCanExecuteChanged;
                }
                element.EnableDisableElement();
            }
        }

        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)base.GetValue(CommandProperty);
            }
            set
            {
                base.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return base.GetValue(CommandParameterProperty);
            }
            set
            {
                base.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This property is here for compatibility
        /// with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="CommandParameter" /> property.
        /// </summary>
        public object CommandParameterValue
        {
            get
            {
                return (this.commandParameterValue ?? this.CommandParameter);
            }
            set
            {
                this.commandParameterValue = value;
                this.EnableDisableElement();
            }
        }

        /// <summary>
        /// Gets or sets a converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand" />. If PassEventArgsToCommand is false,
        /// this property is never used.
        /// </summary>
        public IEventArgsConverter EventArgsConverter { get; set; }

        /// <summary>
        /// Gets or sets a parameters for the converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand" />. If PassEventArgsToCommand is false,
        /// this property is never used. This is a dependency property.
        /// </summary>
        public object EventArgsConverterParameter
        {
            get
            {
                return base.GetValue(EventArgsConverterParameterProperty);
            }
            set
            {
                base.SetValue(EventArgsConverterParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get
            {
                return (bool)base.GetValue(MustToggleIsEnabledProperty);
            }
            set
            {
                base.SetValue(MustToggleIsEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. This property is here for
        /// compatibility with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
        /// </summary>
        public bool MustToggleIsEnabledValue
        {
            get
            {
                if (this.mustToggleValue.HasValue)
                {
                    return this.mustToggleValue.Value;
                }
                return this.MustToggleIsEnabled;
            }
            set
            {
                this.mustToggleValue = value;
                this.EnableDisableElement();
            }
        }

        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this
        /// action should be passed to the bound RelayCommand. If this is true,
        /// the command should accept arguments of the corresponding
        /// type (for example RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }
    }



}
