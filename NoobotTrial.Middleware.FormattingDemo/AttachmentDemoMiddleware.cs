using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using NoobotTrial.Core;

namespace NoobotTrial.Middleware.FormattingDemo
{
    public class AttachmentDemoMiddleware : MiddlewareBase
    {
        private static readonly Regex ParseCommandRegex = new Regex("(?:attachments|atch)\\s+(\\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public AttachmentDemoMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = RegexHandle.For(ParseCommandRegex.ToString()),
                    Description = "attachments demo - try \"atch [keyword]\" (e.g. all, graph)",
                    EvaluatorFunc = Handler
                }
            };
        }

        private IEnumerable<ResponseMessage> Handler(IncomingMessage message, IValidHandle matchedHandle)
        {
            yield return message.IndicateTypingOnChannel();

            var command = ExtractCommand(message.TargetedText);

            switch (command)
            {
                case "all": yield return All(message); break;
                case "graph": yield return Graph(message); break;
            }
        }

        private static ResponseMessage All(IncomingMessage message)
        {
            return message.ReplyToChannel(String.Empty, new Attachment
            {
                Fallback = "Required plain-text summary of the attachment.",
                Color = "#36a64f",
                //Pretext = "Optional text that appears above the attachment block",
                AuthorName = "Author Name",
                //AuthorLink = "https://www.lipsum.com/",
                //AuthorIcon = "https://d30y9cdsu7xlg0.cloudfront.net/png/32324-200.png",
                Title = "Title",
                //TitleLink = "https://groove.hq/path/to/ticket/1943",
                Text = "Optional text that appears within the attachment",
                AttachmentFields = new List<AttachmentField>
                {
                    new AttachmentField
                    {
                        Title = "Priority",
                        Value = "High",
                        IsShort = false
                    }
                },
                ImageUrl = "https://raw.githubusercontent.com/noobot/noobot/master/img/noobot-small.png",
                ThumbUrl = "https://raw.githubusercontent.com/noobot/noobot/master/img/noobot-small.png",
                //Footer = "Slack API",
                //FooterIcon = "https://platform.slack-edge.com/img/default_application_icon.png"
                //Timestamp = new DateTime(2018,05,04)
            });
        }

        private static ResponseMessage Graph(IncomingMessage message)
        {
            return message.ReplyToChannel(String.Empty, new Attachment
            {
                Fallback = "GBP to EUR: How does this look? - https://www.google.com/finance/chart?q=CURRENCY:GBPEUR&chst=vkc&tkr=1&chsc=2&chs=270x94&p=5Yg",
                Title = "GBP to EUR",
                //TitleLink = "https://datadog.com/path/to/event",
                Text = "How does this look?",
                ImageUrl = "https://www.google.com/finance/chart?q=CURRENCY:GBPEUR&chst=vkc&tkr=1&chsc=2&chs=270x94&p=5Yg",
                Color = "#764FA5"
            });
        }

        private static string ExtractCommand(string message)
        {
            return ParseCommandRegex.Match(message).Groups[1].Value.ToLower();
        }
    }
}
