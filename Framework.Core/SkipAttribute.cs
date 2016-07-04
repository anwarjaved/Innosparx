using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Collections.ObjectModel;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SkipAttribute : Attribute
    {
        private readonly Collection<Type> types = new Collection<Type>();

        public SkipAttribute(Type type)
        {
            this.types.Add(type);
        }

        public SkipAttribute(Type type1, Type type2)
        {
            this.types.Add(type1);
            this.types.Add(type2);
        }

        public SkipAttribute(params Type[] types)
        {
            types.ForEach(x => this.types.Add(x));
        }

        public IReadOnlyCollection<Type> Types
        {
            get
            {
                return this.types;
            }
        }
    }
}
