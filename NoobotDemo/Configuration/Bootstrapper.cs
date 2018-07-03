using Common.Logging;
using Noobot.Core;
using Noobot.Core.Configuration;
using Noobot.Core.DependencyResolution;
using NoobotDemo.Middleware.FlightFinder;
using System;

namespace NoobotDemo.Configuration
{
    public static class Bootstrapper
    {
        public static INoobotCore SetupNoobotCore(IConfigReader configurationReader, NoobotDemoConfiguration configuration)
        {
            var logger = new ConsoleOutLogger(
                "",
                LogLevel.All,
                showLevel:true,
                showDateTime:true,
                showLogName:false,
                dateTimeFormat:"yyyy-MM-dd HH:mm:ss");

            var containerFactory = new ContainerFactory(configuration, configurationReader, logger);

            var container = containerFactory.CreateContainer();


            var structuremapContainer = container.GetStructuremapContainer();

            structuremapContainer.Configure(config =>
            {
                config.For<IFlightFinderClient>()
                    .Use(new FlightFinderClient(new Uri("https://flighttrend.azurewebsites.net/")));
            });

            var noobotCore = container.GetNoobotCore();

            return noobotCore;
        }
    }
}
