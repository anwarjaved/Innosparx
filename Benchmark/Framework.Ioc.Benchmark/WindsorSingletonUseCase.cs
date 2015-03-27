using System;
using Castle.Windsor;
using Framework.Ioc.Benchmark.Domain;
using Castle.Core;
using Castle.MicroKernel.Registration;

namespace Framework.Ioc.Benchmark
{
	[System.ComponentModel.Description("WindsorSingleton")]
	public class WindsorSingletonUseCase : UseCase
	{
		static readonly IWindsorContainer container;

		static WindsorSingletonUseCase()
		{
			container = new WindsorContainer();

			Register<IWebService, WebService>();
			Register<IAuthenticator, Authenticator>();
			Register<IStockQuote, StockQuote>();
			Register<IDatabase, Database>();
			Register<IErrorHandler, ErrorHandler>();

			//container.AddComponentWithLifestyle<ILogger, Logger>(LifestyleType.Singleton);
			container.Register(Component.For<ILogger>().ImplementedBy<Logger>().LifeStyle.Is(LifestyleType.Singleton));
		}

		public override void Run()
		{
			var webApp = container.Resolve<IWebService>();
			webApp.Execute();
		}

		static void Register<T, R>()
			where T : class
			where R : T
		{
			var registration = Component.For<T>().ImplementedBy<R>().LifeStyle.Is(LifestyleType.Singleton);
			;
			container.Register(registration);
			//container.AddComponentWithLifestyle(typeof(T).Name, typeof(T), , LifestyleType.Transient);
		}
	}
}
