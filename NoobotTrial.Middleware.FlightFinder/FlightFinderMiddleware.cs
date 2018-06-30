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
                   ValidHandles = ExactMatchHandle.For("tflights"),
                   Description = "Cheapest upcoming weekend flights to Turkey (Istanbul)",
                   EvaluatorFunc = ((Func<IncomingMessage, IValidHandle, IEnumerable<ResponseMessage>>)Handler).WithErrorHandling(log)
               }
            };
        }

        private IEnumerable<ResponseMessage> Handler(IncomingMessage message, IValidHandle matchedHandle)
        {
            var results = _flightFinderClient.Find().GetAwaiter().GetResult();

            var attachments = results
                .OrderBy(x => x.Departure)
                .Take(15)
                .Select(x => new Attachment
                {
                    Text = GetText(x),
                    Color = GetColor(x)
                })
                .ToList();

            attachments.Add(new Attachment
            {
                Fallback = "Book  your flights at https://www.flypgs.com/en",
                Color = "#000"
            }.AddAttachmentAction("Book flights", "https://www.flypgs.com/en"));


            yield return message.ReplyToChannel("Cheapest upcoming weekend flights from London to Istanbul - Friday evening to Sunday evening:", attachments);
        }

        private string GetColor(Flight flight)
        {
            if (flight.Price < 200)
            {
                return "#7CD197";
            }

            if (flight.Price < 400)
            {
                return "#FFFF00";
            }

            return "#FF0000";
        }

        private string GetText(Flight flight)
        {
            return $"*{flight.Price:C}* from _{flight.Departure:g}_ to _{flight.Return:g}_";
        }
    }
}
