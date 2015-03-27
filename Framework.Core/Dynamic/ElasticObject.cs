namespace Framework.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Class ElasticObject.
    /// </summary>
    /// <remarks>
    /// See http://amazedsaint.blogspot.com/2010/02/introducing-elasticobject-for-net-40.html for details
    /// </remarks>
    public class ElasticObject : DynamicObject, IHierarchyWrapperProvider<ElasticObject>, INotifyPropertyChanged
    {
        private readonly IHierarchyWrapperProvider<ElasticObject> elasticProvider = new SimpleHierarchyWrapper();
        private NodeType nodeType = NodeType.Element;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ElasticObject class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public ElasticObject(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "id" + Guid.NewGuid().ToString("N").ToLowerInvariant();
            }

            this.InternalName = name;
        }

        /// <summary>
        /// Initializes a new instance of the ElasticObject class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public ElasticObject(string name, object value)
            : this(name)
        {
            this.InternalValue = value;
        }

        /// <summary>
        /// Initializes a new instance of the ElasticObject class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ElasticObject(object value)
            : this()
        {
            this.InternalValue = value;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the Fully qualified name.
        /// </summary>
        /// <value>The full name of the internal.</value>
        public string InternalFullName
        {
            get
            {
                string path = this.InternalName;
                var parent = this.InternalParent;

                while (parent != null)
                {
                    path = parent.InternalName + "_" + path;
                    parent = parent.InternalParent;
                }

                return path;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the internal value.
        /// </summary>
        /// <value>
        /// The internal value.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public object InternalValue
        {
            get
            {
                return this.elasticProvider.InternalValue;
            }

            set
            {
                this.elasticProvider.InternalValue = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the internal content.
        /// </summary>
        /// <value>
        /// The internal content.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public object InternalContent
        {
            get
            {
                return this.elasticProvider.InternalContent;
            }

            set
            {
                this.elasticProvider.InternalContent = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the name of the internal.
        /// </summary>
        /// <value>
        /// The name of the internal.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string InternalName
        {
            get
            {
                return this.elasticProvider.InternalName;
            }

            set
            {
                this.elasticProvider.InternalName = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal parent.
        /// </summary>
        /// <value>The internal parent.</value>
        public ElasticObject InternalParent
        {
            get
            {
                return this.elasticProvider.InternalParent;
            }

            set
            {
                this.elasticProvider.InternalParent = value;
            }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IEnumerable<KeyValuePair<string, ElasticObject>> Attributes
        {
            get { return this.elasticProvider.Attributes; }
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>The elements.</value>
        public IEnumerable<ElasticObject> Elements
        {
            get { return this.elasticProvider.Elements; }
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, first value in <paramref name="args" /> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        {
            var obj = new ElasticObject(binder.Name, null);

            foreach (var a in args)
            {
                foreach (var p in a.GetType().GetProperties())
                {
                    this.AddAttribute(p.Name, p.GetValue(a, null));
                }
            }

            this.AddElement(obj);
            result = obj;
            return true;
        }

        /// <summary>
        /// Provides implementation for binary operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as addition and multiplication.
        /// </summary>
        /// <param name="binder">Provides information about the binary operation. The binder.Operation property returns an <see cref="T:System.Linq.Expressions.ExpressionType" /> object. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, binder.Operation returns ExpressionType.Add.</param>
        /// <param name="arg">The right operand for the binary operation. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, <paramref name="arg" /> is equal to second.</param>
        /// <param name="result">The result of the binary operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        /// <exception cref="System.InvalidOperationException">An attribute with name + memberName +  already exists</exception>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            if (binder.Operation == ExpressionType.LeftShiftAssign && this.nodeType == NodeType.Element)
            {
                this.InternalContent = arg;
                result = this;
                return true;
            }

            if (binder.Operation == ExpressionType.LeftShiftAssign && this.nodeType == NodeType.Attribute)
            {
                this.InternalValue = arg;
                result = this;
                return true;
            }

            if (binder.Operation == ExpressionType.LeftShift)
            {
                if (arg is string)
                {
                    var exp = new ElasticObject(arg as string, null) { nodeType = NodeType.Element };
                    this.AddElement(exp);
                    result = exp;
                    return true;
                }

                if (arg is ElasticObject)
                {
                    var eobj = arg as ElasticObject;
                    if (!this.Elements.Contains(eobj))
                    {
                        this.AddElement(eobj);
                    }

                    result = eobj;
                    return true;
                }
            }
            else if (binder.Operation == ExpressionType.LessThan)
            {
                string memberName = arg as string;
                if (arg is string)
                {
                    if (!this.HasAttribute(memberName))
                    {
                        var att = new ElasticObject(memberName, null);
                        this.AddAttribute(memberName, att);
                        result = att;
                        return true;
                    }

                    throw new InvalidOperationException("An attribute with name" + memberName + " already exists");
                }

                if (arg is ElasticObject)
                {
                    var eobj = arg as ElasticObject;
                    this.AddAttribute(memberName, eobj);
                    result = eobj;
                    return true;
                }
            }
            else if (binder.Operation == ExpressionType.GreaterThan)
            {
                if (arg is FormatType)
                {
                    result = this.ToXElement();
                    return true;
                }
            }

            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// Provides implementation for unary operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as negation, increment, or decrement.
        /// </summary>
        /// <param name="binder">Provides information about the unary operation. The binder.Operation property returns an <see cref="T:System.Linq.Expressions.ExpressionType" /> object. For example, for the negativeNumber = -number statement, where number is derived from the DynamicObject class, binder.Operation returns "Negate".</param>
        /// <param name="result">The result of the unary operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            if (binder.Operation == ExpressionType.OnesComplement)
            {
                result = (this.nodeType == NodeType.Element) ? this.InternalContent : this.InternalValue;
                return true;
            }

            return base.TryUnaryOperation(binder, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, first value in <paramref name="indexes" /> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if ((indexes.Length == 1) && indexes[0] == null)
            {
                result = this.elasticProvider.Elements.ToList();
            }
            else if ((indexes.Length == 1) && indexes[0] is int)
            {
                var indx = (int)indexes[0];
                var elmt = this.Elements.ElementAt(indx);
                result = elmt;
            }
            else if ((indexes.Length == 1) && indexes[0] is Func<dynamic, bool>)
            {
                var filter = indexes[0] as Func<dynamic, bool>;
                result = this.Elements.Where(c => filter(c)).ToList();
            }
            else
            {
                result = this.Elements.Where(c => indexes.Cast<string>().Contains(c.InternalName)).ToList();
            }

            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.elasticProvider.HasAttribute(binder.Name))
            {
                result = this.elasticProvider.Attribute(binder.Name).InternalValue;
            }
            else
            {
                var obj = this.elasticProvider.Element(binder.Name);
                if (obj != null)
                {
                    result = obj;
                }
                else
                {
                    var exp = new ElasticObject(binder.Name, null);
                    this.elasticProvider.AddElement(exp);
                    result = exp;
                }
            }

            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, the <paramref name="value" /> is "Test".</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;

            this.AddAttribute(memberName, value);

            return true;
        }

        /// <summary>
        /// Query if 'name' has attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>true if attribute, false if not.</returns>
        public bool HasAttribute(string name)
        {
            return this.elasticProvider.HasAttribute(name);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets attribute value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void SetAttributeValue(string name, object obj)
        {
            this.elasticProvider.SetAttributeValue(name, obj);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets attribute value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The attribute value.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public object GetAttributeValue(string name)
        {
            return this.elasticProvider.GetAttributeValue(name);
        }

        /// <summary>
        /// Attributes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>An <see cref="ElasticObject"/>.</returns>
        public ElasticObject Attribute(string name)
        {
            return this.elasticProvider.Attribute(name);
        }

        /// <summary>
        /// Elements the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>An <see cref="ElasticObject"/>.</returns>
        public ElasticObject Element(string name)
        {
            return this.elasticProvider.Element(name);
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddAttribute(string key, ElasticObject value)
        {
            value.nodeType = NodeType.Attribute;
            value.InternalParent = this;
            this.elasticProvider.AddAttribute(key, value);
        }

        /// <summary>
        /// Removes the attribute described by key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveAttribute(string key)
        {
            this.elasticProvider.RemoveAttribute(key);
        }

        /// <summary>
        /// Adds an element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddElement(ElasticObject element)
        {
            element.nodeType = NodeType.Element;
            element.InternalParent = this;
            this.elasticProvider.AddElement(element);
        }

        /// <summary>
        /// Removes the element described by element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void RemoveElement(ElasticObject element)
        {
            this.elasticProvider.RemoveElement(element);
        }

        internal ElasticObject CreateOrGetAttribute(string memberName, object value)
        {
            if (!this.HasAttribute(memberName))
            {
                this.AddAttribute(memberName, new ElasticObject(memberName, value));
            }

            return this.Attribute(memberName);
        }

        private void AddAttribute(string memberName, object value)
        {
            if (value is ElasticObject)
            {
                var eobj = value as ElasticObject;
                if (!this.Elements.Contains(eobj))
                {
                    this.AddElement(eobj);
                }
            }
            else
            {
                if (!this.elasticProvider.HasAttribute(memberName))
                {
                    this.elasticProvider.AddAttribute(memberName, new ElasticObject(memberName, value));
                }
                else
                {
                    this.elasticProvider.SetAttributeValue(memberName, value);
                }
            }

            this.OnPropertyChanged(memberName);
        }

        private void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
