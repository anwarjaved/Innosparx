namespace Framework.Templates.Impl
{
    using System;
    using System.IO;

    using Framework.Ioc;
    using Framework.Reflection;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mustache Template context.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    internal class TemplateContext : DisposableObject, ITemplateContext
    {
        private readonly bool disposeWriter;

        private readonly object instance;

        private readonly IReflectionType reflectionType;

        private readonly Func<string, string> templateLocator;

        private readonly TextWriter writer;

        public TemplateContext(
            TextWriter writer,
            object instance,
            bool disposeWriter = true,
            Func<string, string> templateLocator = null)
        {
            this.writer = writer;
            this.disposeWriter = disposeWriter;
            this.templateLocator = templateLocator;

            if (instance != null)
            {
                this.instance = instance;
                this.reflectionType = Reflector.Get(instance);
            }
        }

        public Func<string, string> TemplateLocator
        {
            get
            {
                return this.templateLocator;
            }
        }

        public ITemplateContext GetContext(object value)
        {
            return new TemplateContext(this.writer, value, false);
        }

        public ICompiledTemplate GetTemplate(string template)
        {
            if (this.templateLocator != null && !string.IsNullOrWhiteSpace(template))
            {
                try
                {
                    string templateValue = this.templateLocator(template);

                    if (!string.IsNullOrWhiteSpace(templateValue))
                    {
                        var engine = Container.Get<ITemplateEngine>();
                        return engine.Compile(templateValue);
                    }
                }
                catch (Exception)
                {
                }
            }

            return null;
        }

        public object GetValue(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && this.reflectionType != null)
            {
                return this.reflectionType.GetPropertyValue(name, this.instance);
            }

            return null;
        }

        public void Write(string text)
        {
            this.writer.Write(text);
        }

        protected override void DisposeResources()
        {
            if (this.disposeWriter)
            {
                this.writer.Dispose();
            }

            base.DisposeResources();
        }
    }
}