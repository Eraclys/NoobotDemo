using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NoobotDemo.Middleware.Authorization
{
    public class PermissionCollection : IEnumerable<Permission>
    {
        private readonly ConcurrentDictionary<Permission, byte> _permissions;

        public PermissionCollection(IEnumerable<Permission> permissions)
        {
            _permissions = new ConcurrentDictionary<Permission, byte>(permissions.Select(x => new KeyValuePair<Permission, byte>(x, 0)));
        }


        public void Add(params Permission[] permissions)
        {
            foreach (var p in permissions)
            {
                _permissions[p] = 0;
            }
        }

        public void Remove(params Permission[] permissions)
        {
            foreach (var p in permissions)
            {
                _permissions.TryRemove(p, out _);
            }
        }

        public bool Contains(Permission permission)
        {
            return _permissions.ContainsKey(permission);
        }

        public IEnumerator<Permission> GetEnumerator()
        {
            return _permissions.Select(x => x.Key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}