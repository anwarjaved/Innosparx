using System;
using System.Linq;
using System.Text;

namespace Framework.Serialization.Xml.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    using Framework.Ioc;
    using Framework.Reflection;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     XML serializer.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(ISerializer), "Xml", LifetimeType.Singleton)]
    [InjectBind(typeof(IXmlSerializer), LifetimeType.Singleton)]
    public class XmlSerializer : IXmlSerializer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes.
        /// </summary>
        ///
        /// <exception cref="NotImplementedException">
        ///     Thrown when the requested operation is unimplemented.
        /// </exception>
        ///
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public object Deserialize(Type type, string value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            IReflectionType reflectionType = Reflector.Get(type);

            object instance = reflectionType.CreateInstance();
            if (!string.IsNullOrWhiteSpace(value))
            {
                XDocument document = XDocument.Parse(value, LoadOptions.SetLineInfo);

                var root = document.Root;

                if (root != null)
                {
                    this.SetRootValue(reflectionType, instance, root);
                }
            }

            return instance;
        }

        private void SetRootValue(IReflectionType reflectionType, object instance, XElement root)
        {
            var properties = reflectionType.Properties.OrderBy(
                p =>
                {
                    OrderAttribute priority = p.Attributes.SingleOrDefault(x => x is OrderAttribute) as OrderAttribute;

                    return priority != null ? priority.Value : int.MaxValue;
                }).ToList();

            SetPropertiesValue(properties, instance, root);
        }

        private void SetPropertiesValue(IEnumerable<IReflectionProperty> properties, object instance, XElement root)
        {
            foreach (var property in properties)
            {
                this.SetProperty(instance, root, property);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public T Deserialize<T>(string value)
        {
            return (T)this.Deserialize(typeof(T), value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/24/2013 9:38 AM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="mode">
        ///     (optional) the mode.
        /// </param>
        /// <param name="nullValue">
        ///     (optional) the null value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string Serialize<T>(T value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            return this.Serialize((object)value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/24/2013 9:38 AM.
        /// </remarks>
        ///
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="mode">
        ///     (optional) the mode.
        /// </param>
        /// <param name="nullValue">
        ///     (optional) the null value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string Serialize(object value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            if (value != null)
            {
                var saveOptions = SaveOptions.OmitDuplicateNamespaces;

                switch (mode)
                {
                    case SerializationMode.Compact:
                        saveOptions |= SaveOptions.DisableFormatting;
                        break;
                }

                XDocument document = new XDocument();
                document.Declaration = new XDeclaration("1.0", "utf-8", null);

                document.AddFirst(BuildRoot(value));
                StringBuilder builder = new StringBuilder();
                using (TextWriter writer = new Framework.IO.StringWriter(Encoding.UTF8, builder))
                {
                    document.Save(writer, saveOptions);
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        private static XElement BuildRoot(object instance)
        {
            IReflectionType reflectionType = Reflector.Get(instance.GetType());
            XElement root = BuildElement(reflectionType);
            var properties = reflectionType.Properties.OrderBy(
                p =>
                {
                    OrderAttribute priority = p.Attributes.SingleOrDefault(x => x is OrderAttribute) as OrderAttribute;

                    return priority != null ? priority.Value : int.MaxValue;
                }).ToList();

            foreach (var property in properties)
            {
                AddProperty(instance, root, property);
            }

            return root;
        }

        private static XElement BuildPropertyRoot(object instance, IReflectionProperty rootProperty)
        {
            IReflectionType reflectionType = Reflector.Get(instance.GetType());
            XElement root = BuildElement(rootProperty);
            var properties = reflectionType.Properties.OrderBy(
                p =>
                {
                    OrderAttribute priority = p.Attributes.SingleOrDefault(x => x is OrderAttribute) as OrderAttribute;

                    return priority != null ? priority.Value : int.MaxValue;
                }).ToList();

            foreach (var property in properties)
            {
                AddProperty(instance, root, property);
            }

            return root;
        }

        private void SetProperty(object instance, XElement root, IReflectionProperty property)
        {
            if (property.IsPrimitive || property.IsNullable)
            {
                if (instance != null)
                {
                    SetPrimitiveProperty(property, instance, root);
                }
            }

            if (property.IsClass)
            {
                var type = Reflector.Get(property.Type);
                object value = null;
                if (property.CanRead)
                {
                    value = property.Get(instance);
                }

                if (value == null)
                {
                    if (property.CanWrite)
                    {
                        value = type.CreateInstance();
                    }
                }

                if (value != null)
                {
                    var properties = type.Properties.OrderBy(
                        p =>
                        {
                            OrderAttribute priority = p.Attributes.SingleOrDefault(x => x is OrderAttribute) as OrderAttribute;

                            return priority != null ? priority.Value : int.MaxValue;
                        }).ToList();

                    XmlPropertyAttribute attribute = (XmlPropertyAttribute)property.Attributes.SingleOrDefault(x => x is XmlPropertyAttribute);

                    var propertyName = property.Name;

                    if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
                    {
                        propertyName = attribute.Name;
                    }

                    var element = root.Element(propertyName);

                    if (element != null)
                    {
                        this.SetPropertiesValue(properties, value, element);
                    }
                }
              
            }
        }

        private static void AddProperty(object instance, XElement root, IReflectionProperty property)
        {
            object value = property.Get(instance);
            if (property.IsPrimitive || property.IsNullable)
            {
                if (value != null)
                {
                    if (property.Type == typeof(string))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
                        {
                            root.Add(BuildPrimitiveProperty(property, value));
                        }
                    }
                    else if (property.Type == typeof(int) || property.Type == typeof(long) || property.Type == typeof(short)
                    || property.Type == typeof(float) || property.Type == typeof(double))
                    {
                        if (Convert.ToDouble(value) != 0)
                        {
                            root.Add(BuildPrimitiveProperty(property, value));
                        }
                    }
                       
                    else
                    {
                        root.Add(BuildPrimitiveProperty(property, value));
                    }
                }
            }

            if (property.IsEnumerable)
            {
                root.Add(property.IsDictionary ? BuildDictionaryProperty(property, value) 
                    : BuildEnumerableProperty(property, value));
            }

            if (property.IsClass)
            {
                root.Add(BuildPropertyRoot(value, property));
            }
        }


        private static XObject BuildEnumerableProperty(IReflectionProperty property, object value)
        {
            XElement root = BuildElement(property);
            if (value != null)
            {
                if (property.EnumerableType.IsPrimitiveType())
                {
                    IEnumerable items = (IEnumerable)value;


                    foreach (var item in items)
                    {
                        root.Add(BuildPrimitiveProperty(property, item));
                    }
                }

            }

            return root;
        }

        private static XObject BuildDictionaryProperty(IReflectionProperty property, object value)
        {
            XElement root = BuildElement(property);
            if (value != null)
            {
                if (property.KeyType.IsPrimitiveType() && property.EnumerableType.IsPrimitiveType())
                {
                    IDictionary items = (IDictionary)value;

                    foreach (DictionaryEntry item in items)
                    {
                        XmlPropertyAttribute attribute = (XmlPropertyAttribute)property.Attributes.SingleOrDefault(x => x is XmlPropertyAttribute);

                        XElement child = BuildElement(property, attribute != null ? attribute.ItemName : null);
                        //child.Add(Bu(property, attribute != null ? attribute.ItemName : null););
                        root.Add(child);
                    }
                }

            }

            return root;
        }


        private void SetPrimitiveProperty(IReflectionProperty property, object instance, XElement root)
        {
            XmlPropertyAttribute attribute = (XmlPropertyAttribute)property.Attributes.SingleOrDefault(x => x is XmlPropertyAttribute);

            var propertyName = property.Name;

            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
            {
                propertyName = attribute.Name;
            }

            var element = root.Element(propertyName);

            if (element != null)
            {
                var value = element.Value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    property.Set(instance, value);
                }
            }
        }

        private static XObject BuildPrimitiveProperty(IReflectionProperty property, object value)
        {
            XmlPropertyAttribute attribute = (XmlPropertyAttribute)property.Attributes.SingleOrDefault(x => x is XmlPropertyAttribute);

            XNamespace defaultNamspace = null;
            List<XAttribute> namespaceList = new List<XAttribute>();
            var nodeName = property.Name;

            if (property.IsEnumerable && !property.IsDictionary && property.EnumerableType != null)
            {
                nodeName = property.EnumerableType.Name;
            }

            if (attribute != null)
            {
                defaultNamspace = attribute.DefaultNamespace;
                namespaceList.AddRange(attribute.NamespaceList.Select(x => new XAttribute(XNamespace.Xmlns + x.Key, x.Value)).ToList());

                if (!string.IsNullOrWhiteSpace(attribute.Name))
                {
                    nodeName = attribute.Name;
                }

                if (property.IsEnumerable && !property.IsDictionary && property.EnumerableType != null && !string.IsNullOrWhiteSpace(attribute.ItemName))
                {
                    nodeName = attribute.ItemName;
                }

            }

            XElement xElement = defaultNamspace != null 
                ? new XElement(defaultNamspace + nodeName, namespaceList) : new XElement(nodeName, namespaceList);

            xElement.SetValue(FormatValue(property, value));
            return xElement;
        }

        private static object FormatValue(IReflectionProperty property, object value)
        {
            if (value != null)
            {
                if (property.Type == typeof(DateTime))
                {
                    return new W3CDateTime((DateTime)value);
                }


            }

            return value;
        }

        private static XElement BuildElement(IReflectionType reflectionType, string elementName = null)
        {
            XmlElementAttribute attribute = (XmlElementAttribute)reflectionType.Attributes.SingleOrDefault(x => x is XmlElementAttribute);

            XNamespace defaultNamspace = null;
            List<XAttribute> namespaceList = new List<XAttribute>();
            var nodeName = reflectionType.Name;
            if (attribute != null)
            {
                defaultNamspace = attribute.DefaultNamespace;
                namespaceList.AddRange(attribute.NamespaceList.Select(x => new XAttribute(XNamespace.Xmlns + x.Key, x.Value)).ToList());

                if (!string.IsNullOrWhiteSpace(attribute.Name))
                {
                    nodeName = attribute.Name;
                }
            }

            if (!string.IsNullOrWhiteSpace(elementName))
            {
                nodeName = elementName;
            }

            XElement xElement = defaultNamspace != null ? 
                new XElement(defaultNamspace + nodeName, namespaceList) : new XElement(nodeName, namespaceList);

            return xElement;
        }

        private static XElement BuildElement(IReflectionProperty property, string elementName = null)
        {
            XmlPropertyAttribute attribute = (XmlPropertyAttribute)property.Attributes.SingleOrDefault(x => x is XmlPropertyAttribute);

            XNamespace defaultNamspace = null;
            List<XAttribute> namespaceList = new List<XAttribute>();
            var nodeName = property.Name;



            if (attribute != null)
            {
                defaultNamspace = attribute.DefaultNamespace;
                namespaceList.AddRange(attribute.NamespaceList.Select(x => new XAttribute(XNamespace.Xmlns + x.Key, x.Value)).ToList());

                if (!string.IsNullOrWhiteSpace(attribute.Name))
                {
                    nodeName = attribute.Name;
                }
            }

            if (!string.IsNullOrWhiteSpace(elementName))
            {
                nodeName = elementName;
            }

            XElement xElement = defaultNamspace != null 
                ? new XElement(defaultNamspace + nodeName, namespaceList) : new XElement(nodeName, namespaceList);

            return xElement;
        }
    }
}
