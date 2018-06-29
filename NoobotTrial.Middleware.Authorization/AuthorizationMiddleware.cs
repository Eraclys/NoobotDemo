using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using System.Collections.Generic;

namespace NoobotTrial.Middleware.Authorization
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly AuthorizationPlugin _authorizationPlugin;
        private readonly IMiddleware _next;

        public AuthorizationMiddleware(AuthorizationPlugin authorizationPlugin, IMiddleware next)
        {
            _authorizationPlugin = authorizationPlugin;
            _next = next;
        }

        protected internal IEnumerable<ResponseMessage> Next(IncomingMessage message)
        {
            return _next?.Invoke(message) ?? new ResponseMessage[0];
        }

        public IEnumerable<ResponseMessage> Invoke(IncomingMessage message)
        {
            var userEmail = message.UserEmail;

            if (message.TargetedText == "authorize all")
            {
                _authorizationPlugin.GrantAccess(userEmail);
                yield return message.ReplyDirectlyToUser("Access granted to everyone.");
                yield break;
            }

            if (message.TargetedText == "deny all")
            {
                _authorizationPlugin.DenyAccess(userEmail);
                yield return message.ReplyDirectlyToUser("Access denied to everyone.");
                yield break;
            }

            if (_authorizationPlugin.IsAuthorizedToExecuteCommands(userEmail))
            {
                foreach (ResponseMessage responseMessage in Next(message))
                {
                    yield return responseMessage;
                }
            }
            else
            {
                yield return message.ReplyDirectlyToUser("Access Denied.");
            }
        }

        public IEnumerable<CommandDescription> GetSupportedCommands()
        {
            yield return new CommandDescription
            {
                Command = "`authorize all`",
                Description = "Grants everyone access"
            };

            yield return new CommandDescription
            {
                Command = "`deny all`",
                Description = "Denies access to everyone"
            };

            foreach (var commandDescription in _next.GetSupportedCommands())
            {
                yield return commandDescription;
            }
        }
    }
}
