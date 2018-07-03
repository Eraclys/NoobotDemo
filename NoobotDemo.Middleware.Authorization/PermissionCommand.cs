using System.Collections.Generic;

namespace NoobotDemo.Middleware.Authorization
{
    public class PermissionCommand
    {
        public OperationType OperationType { get; }
        public string PermissionName { get; }
        public IEnumerable<string> UserEmails { get; }

        public PermissionCommand(OperationType operationType, string permissionName, IEnumerable<string> userEmails)
        {
            OperationType = operationType;
            PermissionName = permissionName;
            UserEmails = userEmails;
        }
    }
}