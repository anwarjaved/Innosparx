using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Framework.Ioc.Benchmark.Domain;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.StaticFactory;

namespace Framework.Ioc.Benchmark
{
	[Description("Unity")]
	public class UnityUseCase : UseCase
	{
		static UnityContainer container;

		static UnityUseCase()
		{
			container = new UnityContainer();

			container.RegisterType<IWebService, WebService>();
			container.RegisterType<IAuthenticator, Authenticator>();
			container.RegisterType<IStockQuote, StockQuote>();
			container.RegisterType<IDatabase, Database>();
			container.RegisterType<IErrorHandler, ErrorHandler>();
			container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());

		}

		public override void Run()
		{
			var webApp = container.Resolve<IWebService>();
			webApp.Execute();
		}
	}
}
