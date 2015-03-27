using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Serialization.Benchmark
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Framework.Activator;
    using Framework.Configuration;
    using Framework.Configuration.Impl;
    using Framework.Ioc;
    using Framework.Serialization.Json;
    using Framework.Serialization.Xml;

    using YAXLib;

    class Program
    {
        static Program()
        {
            ActivationManager.Run();
            Container.Bind<IConfigProvider>().To<XmlConfigProvider>().InSingletonScope();
        }

        static void Main(string[] args)
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/output"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/output");
            }
            var benchMark1 = new Framework.BenchmarkTest();

            benchMark1.AddTest("Run Xml Serialization Test Using YAXLib", RunYaxLibXmlSerialization);

            benchMark1.AddTest("Run Xml Serialization Test", RunXmlSerialization);

            benchMark1.AddTest("Run Json Serialization Test", RunJsonSerialization);


            benchMark1.RunTests(100);

            Console.ReadKey();

            var benchMark2 = new Framework.BenchmarkTest();

            benchMark2.AddTest("Run Json Deserialization Test", RunJsonDeserialization);

            benchMark2.AddTest("Run Xml Deserialization Test Using YAXLib", RunYaxLibXmlDeserialization);

            benchMark2.AddTest("Run Xml Deserialization Test", RunXmlDeserialization);

            benchMark2.RunTests(100);
            Console.ReadKey();
        }

        private static void RunJsonDeserialization()
        {
            Console.WriteLine("Json Deserialization Test");
            IJsonSerializer serializer = Container.Get<IJsonSerializer>();

            Config settings = serializer.Deserialize<Config>(File.ReadAllText(Environment.CurrentDirectory + "/output/test.json"));

            Console.WriteLine(settings != null ? "Success" : "Failure");
            Console.WriteLine();
        }

        private static void RunYaxLibXmlDeserialization()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(Config));
            Console.WriteLine("YaxLib Xml Deserialization Test");

            object settings = serializer.Deserialize(File.ReadAllText(Environment.CurrentDirectory + "/output/yaxlib.xml"));

            Console.WriteLine(settings != null ? "Success" : "Failure");
            Console.WriteLine();
        }

        private static void RunXmlDeserialization()
        {
            Console.WriteLine("Xml Deserialization Test");
            IXmlSerializer serializer = Container.Get<IXmlSerializer>();

            Config settings = serializer.Deserialize<Config>(File.ReadAllText(Environment.CurrentDirectory + "/output/test.xml"));

            Console.WriteLine(settings != null ? "Success" : "Failure");

            Console.WriteLine();
        }

        private static void RunJsonSerialization()
        {
            Console.WriteLine("Json Serialization Test");
            var settings = GetSettings();
            IJsonSerializer serializer = Container.Get<IJsonSerializer>();


            File.WriteAllText(Environment.CurrentDirectory + "/output/test.json", serializer.Serialize(settings));
            Console.WriteLine();
        }

        private static void RunYaxLibXmlSerialization()
        {
            YAXSerializer serializer = new YAXSerializer(typeof(Config));
            Console.WriteLine("YaxLib Xml Serialization Test");
            var settings = GetSettings();

            File.WriteAllText(Environment.CurrentDirectory + "/output/yaxlib.xml", serializer.Serialize(settings));
            Console.WriteLine();
        }

   
        private static Config GetSettings()
        {
            var settingManager = Container.Get<IConfigProvider>().GetConfig();

            if (settingManager == null)
            {
                settingManager = new Config();
            }

            settingManager.Mail.AuthenticationEmail = "test@developer.com";
            settingManager.Mail.SmtpPort = 587;
            settingManager.Amazon.AccessKey = "xxxxxxxxxxxxxxxxxxxx";
            settingManager.Amazon.SecretKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            return settingManager;
        }

        private static void RunXmlSerialization()
        {
            Console.WriteLine("Xml Serialization Test");
            var settings = GetSettings();
            IXmlSerializer serializer = Container.Get<IXmlSerializer>();


            File.WriteAllText(Environment.CurrentDirectory + "/output/test.xml", serializer.Serialize(settings));
            Console.WriteLine();
        }
    }
}
