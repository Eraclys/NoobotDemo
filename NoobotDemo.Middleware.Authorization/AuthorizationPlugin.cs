using Newtonsoft.Json;
using Noobot.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoobotDemo.Middleware.Authorization
{
    public class AuthorizationPlugin : IPlugin
    {
        private static readonly string PermissionsStoreFileName = $"{Environment.CurrentDirectory}\\Authorization.Permissions.json";
        private static readonly object SyncRoot = new object();

        protected Lazy<PermissionCollection> Permissions;

        public AuthorizationPlugin()
        {
            Permissions = new Lazy<PermissionCollection>(Read);
        }

        public void Execute(PermissionCommand command)
        {
            var permissions = command.UserEmails?
                .Select(email => new Permission(command.PermissionName, email))
                .ToArray();

            switch (command.OperationType)
            {
                case OperationType.Grant:
                    Permissions.Value.Add(permissions);
                    break;
                case OperationType.Deny:
                    Permissions.Value.Remove(permissions);
                    break;
            }

            Save();
        }

        public IEnumerable<string> ListUsersByPermissionName(string permissionName)
        {
            return Permissions.Value
                .Where(x => x.Name == permissionName)
                .Select(x => x.User);
        }

        public IEnumerable<string> ListPermissionsByUserEmail(string userEmail)
        {
            return Permissions.Value
                .Where(x => x.User == userEmail)
                .Select(x => x.Name);
        }

        public bool HasPermission(string permissionName, string userEmail)
        {
            return Permissions.Value.Contains(new Permission(permissionName, userEmail));
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            return Permissions.Value.ToList().AsReadOnly();
        }

        private static PermissionCollection Read()
        {
            if (File.Exists(PermissionsStoreFileName))
            {
                var file = File.ReadAllText(PermissionsStoreFileName);

                if (!string.IsNullOrEmpty(file))
                {
                    return new PermissionCollection(JsonConvert.DeserializeObject<IEnumerable<Permission>>(file));
                }
            }

            return new PermissionCollection(Enumerable.Empty<Permission>());
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(Permissions.Value.AsEnumerable(), Formatting.Indented);

            lock (SyncRoot)
            {
                if (!File.Exists(PermissionsStoreFileName))
                {
                    File.Create(PermissionsStoreFileName);
                }

                File.WriteAllText(PermissionsStoreFileName, json);
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
