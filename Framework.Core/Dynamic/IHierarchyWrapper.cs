namespace Framework.Dynamic
{
    using System.Collections.Generic;

    internal interface IHierarchyWrapperProvider<T>
    {
        IEnumerable<KeyValuePair<string, T>> Attributes { get; }

        IEnumerable<T> Elements { get; }

        object InternalValue { get; set; }

        object InternalContent { get; set; }

        string InternalName { get; set; }

        T InternalParent { get; set; }
     
        bool HasAttribute(string name);

        void SetAttributeValue(string name, object obj);

        object GetAttributeValue(string name);

        T Attribute(string name);

        T Element(string name);

        void AddAttribute(string key, T value);

        void RemoveAttribute(string key);

        void AddElement(T element);

        void RemoveElement(T element);
    }
}
