using System;
using System.IO;
using Noobot.Core.Configuration;
using NoobotDemo.Configuration;

namespace NoobotDemo
{
    class Program
    {
        static void Main()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration/config.json");
            var jsonConfigReader = new JsonConfigReader(configPath);
            var middleWareConfiguration = new NoobotDemoConfiguration();

            var noobotCore = Bootstrapper.SetupNoobotCore(jsonConfigReader, middleWareConfiguration);

            var noobotHost = new NoobotHost(noobotCore);

            noobotHost.Start();

            Console.Read();
        }
    }
}
