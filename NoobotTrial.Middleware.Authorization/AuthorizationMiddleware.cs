using Newtonsoft.Json;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using NoobotTrial.Core;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                    Description = "Grants or denies access to a command. Usage: auth grant tflights ebrugul@hotmail.com, john.doe@gmail.com",
                    EvaluatorFunc = HandleGrantOrDenyPermission,
                },
                new HandlerMapping
                {
                    ValidHandles = ExactMatchHandle.For("auth ls"),
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

            var users = _authorizationPlugin.ListUsersByPermissionName(userEmail);
            
            yield return UploadToUser(incomingMessage, users);
        }

        private IEnumerable<ResponseMessage> HandleListPermissionsByUser(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }

            var permissionName = PermissionCommandParser.ParsePermissionsByUserExpression(incomingMessage.TargetedText);

            var permissions = _authorizationPlugin.ListPermissionsByUserEmail(permissionName);

            yield return UploadToUser(incomingMessage, permissions);
        }

        private IEnumerable<ResponseMessage> HandleListAllPermissions(IncomingMessage incomingMessage, IValidHandle validHandle)
        {
            if (!HasAccessToAuthorizationModule(incomingMessage))
            {
                yield return DeniedMessage(incomingMessage); yield break;
            }

            var permissions = _authorizationPlugin.GetAllPermissions();

            yield return UploadToUser(incomingMessage, permissions);
        }

        private IEnumerable<ResponseMessage> HandleGrantOrDenyPermission(IncomingMessage incomingMessage, IValidHandle validHandle)
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

        private static ResponseMessage UploadToUser<T>(IncomingMessage incomingMessage, T value) where T : class
        {
            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });

            var stream = new MemoryStream();

            using (var sw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            using (var jsonTextWriter = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonTextWriter, value);
            }

            stream.Seek(0, SeekOrigin.Begin);

            return incomingMessage.UploadDirectlyToUser(stream, "permissions.json");
        }
    }
}
