﻿using Noobot.Core.Configuration;
using Noobot.Toolbox.Middleware;
using Noobot.Toolbox.Plugins;
using NoobotTrial.Core;
using NoobotTrial.Middleware.Calculator;
using NoobotTrial.Middleware.EnumerableDemo;
using NoobotTrial.Middleware.FlightFinder;
using NoobotTrial.Middleware.FormattingDemo;

namespace NoobotTrial.Configuration
{
    public sealed class NoobotTrialConfiguration : ConfigurationBase
    {
        public NoobotTrialConfiguration()
        {
            UseMiddleware<IndicateTypingMiddleware>();
            UseMiddleware<WelcomeMiddleware>();
            UseMiddleware<JokeMiddleware>();
            UseMiddleware<ScheduleMiddleware>();
            UseMiddleware<FlightFinderMiddleware>();
            UseMiddleware<AttachmentDemoMiddleware>();
            UseMiddleware<CalculatorMiddleware>();
            UseMiddleware<EnumerableDemoMiddleware>();

            UsePlugin<JsonStoragePlugin>();
            UsePlugin<SchedulePlugin>();
        }
    }
}
