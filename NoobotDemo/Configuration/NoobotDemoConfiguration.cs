using Noobot.Core.Configuration;
using Noobot.Toolbox.Middleware;
using Noobot.Toolbox.Plugins;
using NoobotDemo.Middleware.Authorization;
using NoobotDemo.Middleware.Calculator;
using NoobotDemo.Middleware.FlightFinder;
using NoobotDemo.Middleware.FormattingDemo;

namespace NoobotDemo.Configuration
{
    public sealed class NoobotDemoConfiguration : ConfigurationBase
    {
        public NoobotDemoConfiguration()
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
