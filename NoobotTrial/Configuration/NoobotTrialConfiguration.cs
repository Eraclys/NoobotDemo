using Noobot.Core.Configuration;
using Noobot.Toolbox.Middleware;
using Noobot.Toolbox.Plugins;
using NoobotTrial.Middleware.FlightFinder;
using NoobotTrial.Middleware.FormattingDemo;

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
            UseMiddleware<AttachmentDemoMiddleware>();

            UsePlugin<JsonStoragePlugin>();
            UsePlugin<SchedulePlugin>();
        }
    }
}
