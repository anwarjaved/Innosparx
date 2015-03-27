using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Framework.Ioc.Benchmark
{
    class Program
    {
        const int DefaultIterations = 50000;
        private static readonly List<UseCaseInfo> UseCases = new List<UseCaseInfo>
                                                                 {
                                                                     new PlainUseCase(),
                                                                     new MunqUseCase(),
                                                                     new MunqGenericUseCase(),
                                                                     new UnityUseCase(),
                                                                     new AutofacUseCase(),
                                                                     new StructureMapUseCase(),
                                                                     new Ninject2UseCase(),
                                                                     new WindsorUseCase(),
                                                                     new InnoSparxUseCase(),
                                                                 };

        private static readonly List<UseCaseInfo> SingletonUseCases = new List<UseCaseInfo>
                                                                 {
                                                                     new PlainSingletonUseCase(),
                                                                     new MunqSingletonUseCase(),
                                                                     new UnitySingletonUseCase(),
                                                                     new AutofacSingletoncUseCase(),
                                                                     new StructureMapSingletonUseCase(),
                                                                     new Ninject2SingletonUseCase(),
                                                                     new WindsorSingletonUseCase(),
                                                                     new InnoSparxSingletonUseCase()
                                                                 };

        static void Main(string[] args)
        {
            var benchMark = new Framework.BenchmarkTest("Trasient LifeTime");

            foreach (var useCaseInfo in UseCases)
            {
                UseCaseInfo info = useCaseInfo;
                benchMark.AddTest(useCaseInfo.Name, () => info.UseCase.Run());
            }

            benchMark.RunTests(DefaultIterations);

            var benchMark2 = new Framework.BenchmarkTest("Singleton LifeTime");

            foreach (var useCaseInfo in SingletonUseCases)
            {
                UseCaseInfo info = useCaseInfo;
                benchMark2.AddTest(useCaseInfo.Name, () => info.UseCase.Run());
            }

            benchMark2.RunTests(DefaultIterations);
            Console.ReadKey();
        }
    }
}
