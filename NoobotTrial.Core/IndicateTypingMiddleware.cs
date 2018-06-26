using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using System.Collections.Generic;

namespace NoobotTrial.Core
{
    public class IndicateTypingMiddleware : MiddlewareBase
    {
        public IndicateTypingMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[] {new AlwaysMatchHandle()},
                    Description = "Will write typing indicator for each message",
                    EvaluatorFunc = Handler,
                    VisibleInHelp = false,
                    ShouldContinueProcessing = true
                    
                }
            };
        }

        private IEnumerable<ResponseMessage> Handler(IncomingMessage message, IValidHandle matchedHandle)
        {
            yield return message.IndicateTypingOnChannel();
        }
    }
}
