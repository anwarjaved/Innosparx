namespace Framework.Templates.Impl
{
    using System.Collections.Generic;
    using System.Security;
    using System.Text.RegularExpressions;

    using Framework.Dynamic;
    using Framework.Ioc;
    using Framework.Serialization.Json;

    internal class ExpressionPart : ITemplatePart
    {
        private static readonly Regex MarkerRegex = new Regex(
            @"\{([\w]+)\}",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly string expression;

        private readonly bool inverted;

        private readonly ICollection<string> jsonKeys;

        private readonly IEnumerable<ITemplatePart> parts;

        private readonly IDictionary<string, object> properties;

        public ExpressionPart(
            string expression,
            bool inverted,
            IDictionary<string, object> properties,
            ICollection<string> jsonKeys,
            IEnumerable<ITemplatePart> parts)
        {
            this.expression = expression;
            this.inverted = inverted;
            this.properties = properties;
            this.jsonKeys = jsonKeys;
            this.parts = parts;
        }

        
        public void Render(ITemplateContext context)
        {
            var templateExpression = Container.TryGet<ITemplateExpression>(this.expression);

            if (templateExpression != null)
            {
                ExpandedObject clone = this.Build(context);
                templateExpression.Render(this.expression, this.inverted, clone, this.parts, context);
            }
        }

        private ExpandedObject Build(ITemplateContext context)
        {
            IDictionary<string, object> dictionary = new ExpandedObject();
            var serializer = Container.Get<IJsonSerializer>();

            foreach (var property in this.properties)
            {
                if (this.jsonKeys.Contains(property.Key))
                {
                    string values = property.Value.ToString();

                    string value = MarkerRegex.Replace(
                        values,
                        m =>
                            {
                                string propertyName = m.Groups[1].Value;
                                object retValue = context.GetValue(propertyName);
                                return retValue == null ? string.Empty : retValue.ToString();
                            });

                    var parsedValue = serializer.Deserialize<object>(value);
                    dictionary.Add(property.Key, parsedValue);
                }
                else
                {
                    dictionary.Add(property.Key, property.Value);
                }
            }

            return (ExpandedObject)dictionary;
        }
    }
}