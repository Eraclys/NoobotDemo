using System;
using System.Collections.Generic;

namespace NoobotTrial.Middleware.Authorization
{
    public class Permission : IEquatable<Permission>
    {
        public string Name { get; }
        public string User { get; }

        public Permission(string name, string user)
        {
            Name = name;
            User = user;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Permission);
        }

        public bool Equals(Permission other)
        {
            return other != null &&
                   Name == other.Name &&
                   User == other.User;
        }

        public override int GetHashCode()
        {
            var hashCode = 1430559168;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(User);
            return hashCode;
        }
    }
}