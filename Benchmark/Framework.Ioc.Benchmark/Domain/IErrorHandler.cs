using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Ioc.Benchmark.Domain
{
	public interface IErrorHandler
	{
		ILogger Logger { get; }
	}
}
