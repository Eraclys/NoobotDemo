using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;

namespace NoobotDemo.Core
{
    public static class HandlerMappingExtensions
    {
        public static Func<IncomingMessage, IValidHandle, IEnumerable<ResponseMessage>> WithErrorHandling(this Func<IncomingMessage, IValidHandle, IEnumerable<ResponseMessage>> func, ILog logger)
        {
            return (message, handle) =>
            {
                try
                {
                    var results = func(message, handle).ToList();

                    return results;
                }
                catch (Exception e)
                {
                    logger.Error(e);

                    return new[]
                    {
                        message.ReplyToChannel($"Sorry an error occurred {e.Message}")
                    };
                }
            };
        }
    }
}
