namespace Framework.Knockout
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FormatterAttribute : Attribute
    {
        public FormatterAttribute(string formatter)
        {
            this.Formatter = formatter;

        }

        public FormatterAttribute(string formatter, object[] arguments)
        {
            this.Formatter = formatter;
            this.Arguments = arguments;
        }

        public string Formatter { get; private set; }

        public object[] Arguments { get; private set; }
    }
}
