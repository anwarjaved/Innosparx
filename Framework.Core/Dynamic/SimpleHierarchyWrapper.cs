namespace Framework.Dynamic
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A concrete hierarchy wrapper
    /// </summary>
    internal class SimpleHierarchyWrapper : IHierarchyWrapperProvider<ElasticObject>
    {
        private readonly Dictionary<string, ElasticObject> attributes = new Dictionary<string, ElasticObject>();
        private readonly Dictionary<string, List<ElasticObject>> elements = new Dictionary<string, List<ElasticObject>>();

        public IEnumerable<KeyValuePair<string, ElasticObject>> Attributes
        {
            get { return this.attributes; }
        }

        public IEnumerable<ElasticObject> Elements
        {
            get
            {
                var result = from list in this.elements
                             from item in list.Value
                             select item;
                return result;
            }
        }

        public object InternalContent { get; set; }

        public object InternalValue { get; set; }

        public string InternalName { get; set; }

        public ElasticObject InternalParent { get; set; }

        public bool HasAttribute(string name)
        {
            return this.attributes.ContainsKey(name);
        }

        public ElasticObject Attribute(string name)
        {
            if (this.HasAttribute(name))
            {
                return this.attributes[name];
            }

            return null;
        }

        public ElasticObject Element(string name)
        {
            return this.Elements.FirstOrDefault(item => item.InternalName == name);
        }

        public void AddAttribute(string key, ElasticObject value)
        {
            this.attributes.Add(key, value);
        }

        public void RemoveAttribute(string key)
        {
            this.attributes.Remove(key);
        }

        public void AddElement(ElasticObject element)
        {
            if (!this.elements.ContainsKey(element.InternalName))
            {
                this.elements[element.InternalName] = new List<ElasticObject>();
            }

            this.elements[element.InternalName].Add(element);
        }

        public void RemoveElement(ElasticObject element)
        {
            if (this.elements.ContainsKey(element.InternalName))
            {
                if (this.elements[element.InternalName].Contains(element))
                {
                    this.elements[element.InternalName].Remove(element);
                }
            }
        }

        public void SetAttributeValue(string name, object obj)
        {
            this.attributes[name].InternalValue = obj;
        }

        public object GetAttributeValue(string name)
        {
            return this.attributes[name].InternalValue;
        }
    }
}
