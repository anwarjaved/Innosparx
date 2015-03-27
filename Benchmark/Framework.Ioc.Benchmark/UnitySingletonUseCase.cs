using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Framework.Ioc.Benchmark.Domain;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.StaticFactory;

namespace Framework.Ioc.Benchmark
{
	[Description("UnitySingleton")]
	public class UnitySingletonUseCase : UseCase
	{
		static UnityContainer container;

		static UnitySingletonUseCase()
		{
			container = new UnityContainer();

			container.RegisterType<IWebService, WebService>();
			container.RegisterType<IAuthenticator, Authenticator>(new ContainerControlledLifetimeManager());
			container.RegisterType<IStockQuote, StockQuote>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDatabase, Database>(new ContainerControlledLifetimeManager());
			container.RegisterType<IErrorHandler, ErrorHandler>(new ContainerControlledLifetimeManager());
			container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());

		}

		public override void Run()
		{
			var webApp = container.Resolve<IWebService>();
			webApp.Execute();
		}
	}
}
