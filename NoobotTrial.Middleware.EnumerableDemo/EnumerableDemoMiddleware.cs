using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using System.Collections.Generic;
using System.Threading;

namespace NoobotTrial.Middleware.EnumerableDemo
{
    public class EnumerableDemoMiddleware : MiddlewareBase
    {
        public EnumerableDemoMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = ExactMatchHandle.For("demo enumerable"),
                    Description = "Shows that message are streamed one after another",
                    EvaluatorFunc = Handle
                }
            };
        }

        private static IEnumerable<ResponseMessage> Handle(IncomingMessage message, IValidHandle matchedHandle)
        {
            for (var i = 0; i < 4; i++)
            {
                yield return message.ReplyToChannel($"Message {i}");
                Thread.Sleep(3000);
            }
        }
    }
}
