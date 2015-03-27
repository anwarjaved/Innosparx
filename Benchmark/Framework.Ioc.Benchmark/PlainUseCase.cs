using System;
using Framework.Ioc.Benchmark.Domain;
using System.ComponentModel;

namespace Framework.Ioc.Benchmark
{
	[Description("No IOC Container")]
	public class PlainUseCase : UseCase
	{
		public override void Run()
		{
			var logger = new Logger();

			var app = new WebService(
				new Authenticator(
					logger,
					new ErrorHandler(
						logger
					),
					new Database(
						new Logger(),
						new ErrorHandler(
							logger
						)
					)
				),
				new StockQuote(
					logger,
					new ErrorHandler(
						logger
					),
					new Database(
						logger,
						new ErrorHandler(
							logger
						)
					)
				)
			);

			app.Execute();
		}
	}
}
