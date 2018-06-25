using System;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace NoobotTrial
{
    public class ConsoleOutLogger : AbstractSimpleLogger
    {
        public ConsoleOutLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            FormatOutput(stringBuilder, level, message, e);
            Console.WriteLine(stringBuilder.ToString());
        }
    }
}
