using System;
using Framework.Ioc.Benchmark.Domain;
using Framework.Ioc;

namespace Framework.Ioc.Benchmark
{
    [System.ComponentModel.Description("InnoSparxUseCase")]
    public class InnoSparxUseCase : UseCase
    {
        static InnoSparxUseCase()
        {
            Container.Bind<ILogger>("InnoSparxUseCase").To<Logger>().InSingletonScope();
            Container.Bind<IErrorHandler>("InnoSparxUseCase")
                     .ToMethod(() => new ErrorHandler(Container.Get<ILogger>("InnoSparxUseCase")))
                     .InTransientScope();
            Container.Bind<IDatabase>("InnoSparxUseCase")
                     .ToMethod(
                         () =>
                         new Database(
                             Container.Get<ILogger>("InnoSparxUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxUseCase")))
                     .InTransientScope();

            Container.Bind<IAuthenticator>("InnoSparxUseCase")
                     .ToMethod(
                         () =>
                         new Authenticator(
                             Container.Get<ILogger>("InnoSparxUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxUseCase"),
                             Container.Get<IDatabase>("InnoSparxUseCase")))
                     .InTransientScope();

            Container.Bind<IStockQuote>("InnoSparxUseCase").ToMethod(
                         () =>
                         new StockQuote(
                             Container.Get<ILogger>("InnoSparxUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxUseCase"),
                             Container.Get<IDatabase>("InnoSparxUseCase")))
                     .InTransientScope();

            Container.Bind<IWebService>("InnoSparxUseCase").ToMethod(
                         () =>
                         new WebService(
                             Container.Get<IAuthenticator>("InnoSparxUseCase"),
                             Container.Get<IStockQuote>("InnoSparxUseCase")))
                     .InTransientScope();

        }
        public override void Run()
        {
            var webApp = Container.Get<IWebService>("InnoSparxUseCase");
            webApp.Execute();
        }
    }
}
