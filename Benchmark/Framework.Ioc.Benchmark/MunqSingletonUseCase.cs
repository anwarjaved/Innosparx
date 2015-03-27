namespace Framework.Ioc.Benchmark
{
    using System;

    using Framework.Ioc.Benchmark.Domain;

    using Munq;

    [System.ComponentModel.Description("MunqSingleton")]
    public class MunqSingletonUseCase : UseCase
    {
        static IocContainer container;
        static ILifetimeManager singleton;

        static MunqSingletonUseCase()
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
                    c.Resolve<IDatabase>()))
                    .WithLifetimeManager(singleton);

            container.Register<IStockQuote>(
                c => new StockQuote(
                    c.Resolve<ILogger>(),
                    c.Resolve<IErrorHandler>(),
                    c.Resolve<IDatabase>()))
                    .WithLifetimeManager(singleton);

            container.Register<IDatabase>(
                c => new Database(
                    c.Resolve<ILogger>(),
                    c.Resolve<IErrorHandler>()))
                    .WithLifetimeManager(singleton);

            container.Register<IErrorHandler>(
                c => new ErrorHandler(c.Resolve<ILogger>()))
                    .WithLifetimeManager(singleton);

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
