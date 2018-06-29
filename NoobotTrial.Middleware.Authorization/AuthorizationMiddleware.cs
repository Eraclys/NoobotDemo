using Newtonsoft.Json;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using NoobotTrial.Core;
using System.Collections.Generic;

namespace NoobotTrial.Middleware.Authorization
{
    public class AuthorizationMiddleware : MiddlewareBase
    {
        private readonly AuthorizationPlugin _authorizationPlugin;

        public AuthorizationMiddleware(AuthorizationPlugin authorizationPlugin, IMiddleware next) : base(next)
        {
            _authorizationPlugin = authorizationPlugin;

            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = ValidHandle.For(
                        "auth (grant|deny) [permissionName] [userEmail]", 
                        x => PermissionCommandParser.TryParsePermissionCommandExpression(x).WasSuccessful),
                    Description = "Grants or denies access to a command",
                    EvaluatorFunc = HandlePermissionCommand,
                },
                new HandlerMapping
                {
                    ValidHandles = ExactMatchHandle.For("auth ls -all"),
                    Description = "Will provide the full list of permissions",
                    EvaluatorFunc = HandleListAllPermissions
                },
                new HandlerMapping
                {
                    ValidHandles = ValidHandle.For(
                        "auth ls -u [userEmail]",
                        x => PermissionCommandParser.TryParsePermissionsByUserExpression(x).WasSuccessful),
                    Description = "Will provide the full list of permissions for a user",
                    EvaluatorFunc = HandleListPermissionsByUser
                },
                new HandlerMapping
                {
                    ValidHandles = ValidHandle.For(
                        "auth ls -p [permissionName]",
                        x => PermissionCommandParser.TryParseUsersByPermissionExpression(x).WasSuccessful),
                    Description = "Will provide the full list of users who have the specified permission",
                    EvaluatorFunc = HandleListUsersByPermissions
                }
            };
        }

        private IEnumerable<ResponseMessage> HandleListUsersByPermissions(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }
            
            var userEmail = PermissionCommandParser.ParseUsersByPermissionExpression(incomingMessage.TargetedText);

            var allPermissions = _authorizationPlugin.ListUsersByPermissionName(userEmail);

            var json = JsonConvert.SerializeObject(allPermissions, Formatting.Indented);

            yield return incomingMessage.ReplyToChannel(json);
        }

        private IEnumerable<ResponseMessage> HandleListPermissionsByUser(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }

            var permissionName = PermissionCommandParser.ParsePermissionsByUserExpression(incomingMessage.TargetedText);

            var allPermissions = _authorizationPlugin.ListPermissionsByUserEmail(permissionName);

            var json = JsonConvert.SerializeObject(allPermissions, Formatting.Indented);

            yield return incomingMessage.ReplyToChannel(json);
        }

        private IEnumerable<ResponseMessage> HandleListAllPermissions(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }

            var allPermissions = _authorizationPlugin.GetAllPermissions();
            var json = JsonConvert.SerializeObject(allPermissions, Formatting.Indented);

            yield return incomingMessage.ReplyToChannel(json);
        }

        private IEnumerable<ResponseMessage> HandlePermissionCommand(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            var command = PermissionCommandParser.ParsePermissionCommandExpression(incomingMessage.TargetedText);

            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }
            
            _authorizationPlugin.Execute(command);
            
            switch (command.OperationType)
            {

                case OperationType.Grant:
                    yield return incomingMessage.ReplyToChannel("Permission(s) granted.");
                    break;
                case OperationType.Deny:
                    yield return incomingMessage.ReplyToChannel("Permission(s) revoked.");
                    break;
            }
        }

        private bool HasAccessToAuthorizationModule(IncomingMessage incomingMessage)
        {
            return _authorizationPlugin.HasPermission("auth", incomingMessage.UserEmail);
        }

        private static ResponseMessage DeniedMessage(IncomingMessage incomingMessage)
        {
            return incomingMessage.ReplyToChannel("Not on my watch!");
        }
    }
}
