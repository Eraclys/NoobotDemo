using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using Sprache;
using System.Collections.Generic;
using System.Globalization;

namespace NoobotDemo.Middleware.Calculator
{
    public class CalculatorMiddleware : MiddlewareBase
    {
        public CalculatorMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = StartsWithHandle.For("calc "),
                    Description = "Try calc [math expression]",
                    EvaluatorFunc = Handle
                }
            };
        }

        private IEnumerable<ResponseMessage> Handle(IncomingMessage message, IValidHandle matchedHandle)
        {
            var expression = message.TargetedText.Replace("calc ", string.Empty);

            var responses = new List<string>();

            try
            {
                var func = new SimpleCalculator().ParseExpression(expression).Compile();

                var result = func();

                responses.Add(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (ParseException e)
            {
                responses.Add(e.Message);
            }

            foreach (var response in responses)
            {
                yield return message.ReplyToChannel(response);
            }
        }
    }
}
