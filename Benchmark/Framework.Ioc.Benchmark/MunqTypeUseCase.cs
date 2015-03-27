using System;
using Framework.Ioc.Benchmark.Domain;
using Munq;
using Munq.LifetimeManagers;

namespace Framework.Ioc.Benchmark
{
	[System.ComponentModel.Description("MunqGeneric")]
	public class MunqGenericUseCase : UseCase
	{
		static IocContainer container;
		static ILifetimeManager singleton = new ContainerLifetime();

		static MunqGenericUseCase()
		{
			container = new IocContainer();

			//container.Register<IWebService, WebService>();
			container.Register<IAuthenticator, Authenticator>();
			container.Register<IStockQuote, StockQuote>();
			container.Register<IDatabase, Database>();
			container.Register<IErrorHandler, ErrorHandler>();
			container.Register<ILogger,Logger>().WithLifetimeManager(singleton);
		}

		public override void Run()
		{
			var webApp = container.Resolve<WebService>();
			webApp.Execute();
		}
	}
}
