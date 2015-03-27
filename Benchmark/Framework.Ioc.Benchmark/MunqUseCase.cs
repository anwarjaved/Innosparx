using System;
using Framework.Ioc.Benchmark.Domain;
using Munq;

namespace Framework.Ioc.Benchmark
{
	[System.ComponentModel.Description("Munq")]
	public class MunqUseCase : UseCase
	{
		static IocContainer container;
		static ILifetimeManager singleton;

		static MunqUseCase()
		{
			container = new IocContainer();
			singleton = new Munq.LifetimeManagers.ContainerLifetime();

			container.Register<IWebService>(
				c => new WebService(
					c.Resolve<IAuthenticator>(),
					c.Resolve<IStockQuote>()));

			container.Register<IAuthenticator>(
				c => new Authenticator(
					c.Resolve<ILogger>(),
					c.Resolve<IErrorHandler>(),
					c.Resolve<IDatabase>()));

			container.Register<IStockQuote>(
				c => new StockQuote(
					c.Resolve<ILogger>(),
					c.Resolve<IErrorHandler>(),
					c.Resolve<IDatabase>()));

			container.Register<IDatabase>(
				c => new Database(
					c.Resolve<ILogger>(),
					c.Resolve<IErrorHandler>()));

			container.Register<IErrorHandler>(
				c => new ErrorHandler(c.Resolve<ILogger>()));

			container.RegisterInstance<ILogger>(new Logger())
					.WithLifetimeManager(singleton);
		}

		public override void Run()
		{
			var webApp = container.Resolve<IWebService>();
			webApp.Execute();
		}
	}
}
