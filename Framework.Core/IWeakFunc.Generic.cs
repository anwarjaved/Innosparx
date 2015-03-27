using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IWeakFunc<in T, out TResult> : IWeakFunc<TResult>
    {
        TResult Execute(T parameter);
    }
}
