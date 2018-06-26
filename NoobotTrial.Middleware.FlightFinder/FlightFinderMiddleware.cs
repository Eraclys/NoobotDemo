using Common.Logging;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using NoobotTrial.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoobotTrial.Middleware.FlightFinder
{
    public class FlightFinderMiddleware : MiddlewareBase
    {
        private readonly IFlightFinderClient _flightFinderClient;

        public FlightFinderMiddleware(
            IMiddleware next,
            IFlightFinderClient flightFinderClient,
            ILog log) : base(next)
        {
            _flightFinderClient = flightFinderClient;
            HandlerMappings = new[]
            {
               new HandlerMapping
               {
                   ValidHandles = ExactMatchHandle.For("flights"),
                   Description = "Cheapest upcoming flights to Turkey",
                   EvaluatorFunc = ((Func<IncomingMessage, IValidHandle, IEnumerable<ResponseMessage>>)Handler).WithErrorHandling(log)
               }
            };
        }

        private IEnumerable<ResponseMessage> Handler(IncomingMessage message, IValidHandle matchedHandle)
        {
            var results = _flightFinderClient.Find().GetAwaiter().GetResult();

            var attachments = results
                .OrderBy(x => x.Departure)
                .Select(x => new Attachment
                {
                    Text = GetText(x),
                    Color = GetColor(x)
                })
                .ToList();

            yield return message.ReplyToChannel("Here is what we found", attachments);
        }

        private string GetColor(Flight flight)
        {
            if (flight.Price < 100)
            {
                return "#7CD197";
            }

            if (flight.Price < 200)
            {
                return "#F35A00";
            }

            return "#FF0000";
        }

        private string GetText(Flight flight)
        {
            return $"*{flight.Price:C}* from _{flight.Departure:g}_ to _{flight.Return:g}_";
        }
    }
}
