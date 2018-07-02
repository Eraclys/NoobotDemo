using Noobot.Core.Configuration;
using Noobot.Toolbox.Middleware;
using Noobot.Toolbox.Plugins;
using NoobotTrial.Middleware.Authorization;
using NoobotTrial.Middleware.Calculator;
using NoobotTrial.Middleware.FlightFinder;
using NoobotTrial.Middleware.FormattingDemo;

namespace NoobotTrial.Configuration
{
    public sealed class NoobotTrialConfiguration : ConfigurationBase
    {
        public NoobotTrialConfiguration()
        {
            //UseMiddleware<IndicateTypingMiddleware>();
            UseMiddleware<AuthorizationMiddleware>();
            UseMiddleware<WelcomeMiddleware>();
            UseMiddleware<JokeMiddleware>();
            UseMiddleware<ScheduleMiddleware>();
            UseMiddleware<FlightFinderMiddleware>();
            UseMiddleware<AttachmentDemoMiddleware>();
            UseMiddleware<CalculatorMiddleware>();
            UseMiddleware<YieldTestMiddleware>();

            UsePlugin<JsonStoragePlugin>();
            UsePlugin<SchedulePlugin>();
            UsePlugin<AuthorizationPlugin>();
        }
    }
}
