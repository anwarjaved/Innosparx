using System;
using Framework.Ioc.Benchmark.Domain;
using Framework.Ioc;

namespace Framework.Ioc.Benchmark
{
    [System.ComponentModel.Description("InnoSparxSingletonUseCase")]
    public class InnoSparxSingletonUseCase : UseCase
    {
        static InnoSparxSingletonUseCase()
        {
            Container.Bind<ILogger>("InnoSparxSingletonUseCase").To<Logger>().InSingletonScope();
            Container.Bind<IErrorHandler>("InnoSparxSingletonUseCase")
                     .ToMethod(() => new ErrorHandler(Container.Get<ILogger>("InnoSparxSingletonUseCase")))
                     .InSingletonScope();
            Container.Bind<IDatabase>("InnoSparxSingletonUseCase")
                     .ToMethod(
                         () =>
                         new Database(
                             Container.Get<ILogger>("InnoSparxSingletonUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxSingletonUseCase")))
                     .InSingletonScope();

            Container.Bind<IAuthenticator>("InnoSparxSingletonUseCase")
                     .ToMethod(
                         () =>
                         new Authenticator(
                             Container.Get<ILogger>("InnoSparxSingletonUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxSingletonUseCase"),
                             Container.Get<IDatabase>("InnoSparxSingletonUseCase")))
                     .InSingletonScope();

            Container.Bind<IStockQuote>("InnoSparxSingletonUseCase").ToMethod(
                         () =>
                         new StockQuote(
                             Container.Get<ILogger>("InnoSparxSingletonUseCase"),
                             Container.Get<IErrorHandler>("InnoSparxSingletonUseCase"),
                             Container.Get<IDatabase>("InnoSparxSingletonUseCase")))
                     .InSingletonScope();

            Container.Bind<IWebService>("InnoSparxSingletonUseCase").ToMethod(
                         () =>
                         new WebService(
                             Container.Get<IAuthenticator>("InnoSparxSingletonUseCase"),
                             Container.Get<IStockQuote>("InnoSparxSingletonUseCase")))
                     .InTransientScope();

        }

        public override void Run()
        {
            var webApp = Container.Get<IWebService>("InnoSparxSingletonUseCase");
            webApp.Execute();
        }
    }
}
