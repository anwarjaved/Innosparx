using System;
using Ninject;
using Ninject.Modules;
using Framework.Ioc.Benchmark.Domain;
using System.ComponentModel;

namespace Framework.Ioc.Benchmark
{
	[Description("Ninject2Singleton")]
	public class Ninject2SingletonUseCase : UseCase
	{
		static IKernel kernel;

		static Ninject2SingletonUseCase()
		{
			kernel = new Ninject.StandardKernel(new SampleModule());
		}

		public override void Run()
		{
			var webApp = kernel.Get<IWebService>();
			webApp.Execute();
		}

		class SampleModule : NinjectModule
		{
			public override void Load()
			{
				Bind<IWebService>().ToMethod(
					c => new WebService(
						c.Kernel.Get<IAuthenticator>(),
						c.Kernel.Get<IStockQuote>()))
					.InTransientScope();

				Bind<IAuthenticator>().ToMethod(
					c => new Authenticator(
						c.Kernel.Get<ILogger>(),
						c.Kernel.Get<IErrorHandler>(),
						c.Kernel.Get<IDatabase>()))
					.InSingletonScope();

				Bind<IStockQuote>().ToMethod(
					c => new StockQuote(
						c.Kernel.Get<ILogger>(),
						c.Kernel.Get<IErrorHandler>(),
						c.Kernel.Get<IDatabase>()))
					.InSingletonScope();

				Bind<IDatabase>().ToMethod(
					c => new Database(
						c.Kernel.Get<ILogger>(),
						c.Kernel.Get<IErrorHandler>()))
					.InSingletonScope();

				Bind<IErrorHandler>().ToMethod(
					c => new ErrorHandler(c.Kernel.Get<ILogger>()))
					.InSingletonScope();

				Bind<ILogger>().ToConstant(new Logger())
					.InSingletonScope();
			}
		}
	}
}
