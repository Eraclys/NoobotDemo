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
        private static readonly Regex ParseCommandRegex = new Regex("demo (?:attachments|atch)\\s+(\\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public AttachmentDemoMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new RegexHandle(ParseCommandRegex.ToString(), "demo (atch|attachement) [keyword]")
                    },
                    Description = "Attachments demo - keywords `book` | `graph`",
                    EvaluatorFunc = Handler
                }
            };
        }

        private IEnumerable<ResponseMessage> Handler(IncomingMessage message, IValidHandle matchedHandle)
        {
            var command = ExtractCommand(message.TargetedText);

            switch (command)
            {
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
                    Fallback = "Writing High-Performance .NET Code https://www.amazon.co.uk/Writing-High-Performance-NET-Code-Watson/dp/0990583430",
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
                    AuthorIcon = "https://pbs.twimg.com/profile_images/488797616277184512/mbzx2JMe_400x400.jpeg",
                    ThumbUrl = "https://www.writinghighperf.net/wp-content/uploads/2014/07/cropped-Banner.jpg",
                    ImageUrl = "https://www.writinghighperf.net/wp-content/uploads/2018/03/Cover-Epub-300x370.jpg"
                },
                new Attachment
                {
                    Pretext = "What do readers say?",
                    Text = "“This book is incredibly well-crafted…Ben’s style provides layers to these topics so the reader can choose how deep to go while still gaining valuable insights.” 5 stars – T. Segal"
                },
                new Attachment
                {
                    Title = "Would you recommend it to your colleagues?",
                    Color = "#3AA3E3"
                }
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
