using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            var command = ExtractCommand(message.TargetedText);

            switch (command)
            {
                case "all": yield return All(message); break;
                case "graph": yield return Graph(message); break;
                case "book": yield return Book(message); break;
            }
        }

        private ResponseMessage Book(IncomingMessage message)
        {
            return message.ReplyToChannel("Great programming book", new List<Attachment>
            {
                new Attachment
                {
                    Title = "Writing High-Performance .NET Code",
                    TitleLink = "https://www.amazon.co.uk/Writing-High-Performance-NET-Code-Watson/dp/0990583430",
                    AttachmentFields = new List<AttachmentField>
                    {
                        new AttachmentField
                        {
                            IsShort = true,
                            Title = "Edition",
                            Value = "2"
                        },
                        new AttachmentField
                        {
                            IsShort = true,
                            Title = "Paperback",
                            Value = "Yes"
                        },
                        new AttachmentField
                        {
                            IsShort = true,
                            Title = "Year",
                            Value = "2014"
                        }
                    },
                    AuthorName = "Ben Watson",
                    AuthorLink = "https://www.writinghighperf.net/",
                    ImageUrl = "https://www.writinghighperf.net/wp-content/uploads/2018/03/Cover-Epub-300x370.jpg"
                },
                new Attachment
                {
                    Title = "What do readers say?",
                    Text = "“This book is incredibly well-crafted…Ben’s style provides layers to these topics so the reader can choose how deep to go while still gaining valuable insights.” 5 stars – T. Segal"
                },
                new Attachment
                {
                    Title = "Would you recommend it to your colleagues?",
                    Color = "#3AA3E3"
                }
            });
        }

        private static ResponseMessage All(IncomingMessage message)
        {
            return message.ReplyToChannel(String.Empty, new Attachment
            {
                Fallback = "Required plain-text summary of the attachment.",
                Color = "#36a64f",
                Pretext = "Optional text that appears above the attachment block",
                AuthorName = "Author Name",
                AuthorLink = "https://www.lipsum.com/",
                AuthorIcon = "https://d30y9cdsu7xlg0.cloudfront.net/png/32324-200.png",
                Title = "Title",
                TitleLink = "https://stackoverflow.com/",
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
                //FooterIcon = "https://platform.slack-edge.com/img/default_application_icon.png",
                //Timestamp = new DateTime(2018,05,04)
            });
        }

        private static ResponseMessage Graph(IncomingMessage message)
        {
            return message.ReplyToChannel(String.Empty, new Attachment
            {
                Fallback = "GBP to EUR: How does this look? - https://www.google.com/finance/chart?q=CURRENCY:GBPEUR&chst=vkc&tkr=1&chsc=2&chs=270x94&p=5Y",
                Title = "GBP to EUR",
                TitleLink = "https://www.google.com/finance",
                Text = "How does this look?",
                ImageUrl = "https://www.google.com/finance/chart?q=CURRENCY:GBPEUR&chst=vkc&tkr=1&chsc=2&chs=270x94&p=5Y",
                Color = "#764FA5"
            });
        }

        private static string ExtractCommand(string message)
        {
            return ParseCommandRegex.Match(message).Groups[1].Value.ToLower();
        }
    }
}
