namespace Framework
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Xml.Linq;

    using Framework.Dynamic;
    
    /// <summary>
    /// Extension methods for our ElasticObject. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DynamicExtensions
    {
        /// <summary>
        /// Converts an <see cref="XElement" /> to the <see cref="ExpandedObject"/>.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="instance">The object to convert.</param>
        /// <returns>instance as an <see cref="ExpandedObject"/>.</returns>
        public static ExpandedObject ToExpando<T>(this T instance) where T : class
        {
            IDictionary<string, object> expando = new Dictionary<string, object>();

            if (instance != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(instance.GetType()))
                {
                    expando.Add(property.Name, property.GetValue(instance));
                }
            }

            return new ExpandedObject(expando);
        }

        /// <summary>
        /// Converts an <see cref="XElement" /> to the <see cref="ElasticObject"/>.
        /// </summary>
        /// <param name="e">The <see cref="XElement"/> object.</param>
        /// <returns>Converted <see cref="ElasticObject"/> object.</returns>
        public static ElasticObject ToElastic(this XElement e)
        {
            return ElasticFromXElement(e);
        }
        
        /// <summary>
        /// Converts an <see cref="ElasticObject"/> to XElement.
        /// </summary>
        /// <param name="e">The <see cref="ElasticObject"/>.</param>
        /// <returns>Converted <see cref="XElement"/> object.</returns>
        public static XElement ToXElement(this ElasticObject e)
        {
            return XElementFromElastic(e);
        }

        internal static dynamic WrapObject(object value)
        {
            // The JavaScriptSerializer returns IDictionary<string, object> for objects
            // and object[] for arrays, so we wrap those in different dynamic objects
            // so we can access the object graph using dynamic
            var dictionaryValues = value as IDictionary<string, object>;
            if (dictionaryValues != null)
            {
                return new ExpandedObject(dictionaryValues);
            }

            var arrayValues = value as object[];
            if (arrayValues != null)
            {
                return new ExpandedArray(arrayValues);
            }

            return value;
        }

        private static ElasticObject ElasticFromXElement(XElement el)
        {
            var exp = new ElasticObject();

            if (!string.IsNullOrEmpty(el.Value))
            {
                exp.InternalValue = el.Value;
            }

            exp.InternalName = el.Name.LocalName;

            foreach (var a in el.Attributes())
            {
                exp.CreateOrGetAttribute(a.Name.LocalName, a.Value);
            }

            var textNode = el.Nodes().FirstOrDefault();
            if (textNode is XText)
            {
                exp.InternalContent = textNode.ToString();
            }

            foreach (var c in el.Elements())
            {
                var child = ElasticFromXElement(c);
                child.InternalParent = exp;
                exp.AddElement(child);
            }

            return exp;
        }

        private static XElement XElementFromElastic(ElasticObject elastic)
        {
            var exp = new XElement(elastic.InternalName);

            foreach (var a in elastic.Attributes)
            {
                if (a.Value.InternalValue != null)
                {
                    exp.Add(new XAttribute(a.Key, a.Value.InternalValue));
                }
            }

            if (elastic.InternalContent is string)
            {
                exp.Add(new XText(elastic.InternalContent as string));
            }

            foreach (var c in elastic.Elements)
            {
                var child = XElementFromElastic(c);
                exp.Add(child);
            }

            return exp;
        }
    }
}