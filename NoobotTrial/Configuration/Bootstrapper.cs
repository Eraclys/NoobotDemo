using System;
using Common.Logging;
using Noobot.Core;
using Noobot.Core.Configuration;
using Noobot.Core.DependencyResolution;
using NoobotTrial.Middleware.FlightFinder;

namespace NoobotTrial.Configuration
{
    public static class Bootstrapper
    {
        public static INoobotCore SetupNoobotCore(IConfigReader configurationReader, NoobotTrialConfiguration configuration)
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
