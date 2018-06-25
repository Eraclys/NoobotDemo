using Noobot.Core.Configuration;
using Noobot.Toolbox.Middleware;
using Noobot.Toolbox.Plugins;
using NoobotTrial.Middleware.FlightFinder;

namespace NoobotTrial.Configuration
{
    public sealed class NoobotTrialConfiguration : ConfigurationBase
    {
        public NoobotTrialConfiguration()
        {
            UseMiddleware<WelcomeMiddleware>();
            UseMiddleware<JokeMiddleware>();
            UseMiddleware<ScheduleMiddleware>();
            UseMiddleware<FlightFinderMiddleware>();

            UsePlugin<JsonStoragePlugin>();
            UsePlugin<SchedulePlugin>();
        }
    }
}
