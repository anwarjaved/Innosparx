using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Ioc.Benchmark.Domain
{
	public interface IWebService
	{
		IAuthenticator Authenticator { get; }
		IStockQuote StockQuote { get; }
		void Execute();
	}
}