namespace Framework.Templates.Impl
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Security;

    internal class SectionPart : ITemplatePart
    {
        private readonly bool inverted;

        private readonly Collection<ITemplatePart> parts;

        private readonly string sectionName;

        public SectionPart(string sectionName, bool inverted, Collection<ITemplatePart> parts)
        {
            this.sectionName = sectionName;
            this.inverted = inverted;
            this.parts = parts;
        }

        public Collection<ITemplatePart> Parts
        {
            get
            {
                return this.parts;
            }
        }

        [SecurityCritical]
        public void Render(ITemplateContext context)
        {
            object value = context.GetValue(this.sectionName);

            double numericValue;
            DateTime dateValue;

            if (!this.inverted)
            {
                if (value != null)
                {
                    Type valueType = value.GetType();

                    if (valueType == typeof(string))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(bool))
                    {
                        if (Convert.ToBoolean(value))
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(Guid))
                    {
                        if (new Guid(value.ToString()) != Guid.Empty)
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(value.ToString(), out dateValue))
                        {
                            if (dateValue > DateTime.MinValue)
                            {
                                this.Parts.Render(context);
                            }
                        }
                    }
                    else if (valueType == typeof(int) || valueType == typeof(short) || valueType == typeof(byte)
                             || valueType == typeof(long) || valueType == typeof(double) || valueType == typeof(float))
                    {
                        if (double.TryParse(value.ToString(), out numericValue))
                        {
                            if (numericValue > 0)
                            {
                                this.Parts.Render(context);
                            }
                        }
                    }
                    else if (valueType.IsEnumerable())
                    {
                        foreach (object item in (IEnumerable)value)
                        {
                            ITemplateContext newContext = context.GetContext(item);
                            this.Parts.Render(newContext);
                        }
                    }

                    else
                    {
                        ITemplateContext newContext = context.GetContext(value);
                        this.Parts.Render(newContext);
                    }
                }
            }
            else
            {
                if (value == null)
                {
                    foreach (ITemplatePart templatePart in this.Parts)
                    {
                        templatePart.Render(context);
                    }
                }
                else
                {
                    Type valueType = value.GetType();

                    if (valueType == typeof(string))
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(bool))
                    {
                        if (!Convert.ToBoolean(value))
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(Guid))
                    {
                        if (new Guid(value.ToString()) == Guid.Empty)
                        {
                            this.Parts.Render(context);
                        }
                    }
                    else if (valueType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(value.ToString(), out dateValue))
                        {
                            if (dateValue == DateTime.MinValue)
                            {
                                this.Parts.Render(context);
                            }
                        }
                    }
                    else if (valueType == typeof(int) || valueType == typeof(short) || valueType == typeof(byte)
                             || valueType == typeof(long) || valueType == typeof(double) || valueType == typeof(float))
                    {
                        if (double.TryParse(value.ToString(), out numericValue))
                        {
                            if (numericValue == 0)
                            {
                                this.Parts.Render(context);
                            }
                        }
                    }
                    else if (valueType.IsEnumerable())
                    {
                        var enumerable = (IEnumerable)value;

                        if (enumerable.GetCount() == 0)
                        {
                            this.Parts.Render(context);
                        }
                    }
                }
            }
        }
    }
}