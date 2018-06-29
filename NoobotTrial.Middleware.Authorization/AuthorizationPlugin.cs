using Noobot.Core.Plugins;

namespace NoobotTrial.Middleware.Authorization
{
    public class AuthorizationPlugin : IPlugin
    {
        private bool _isAuthorized = false;

        public void Start()
        {
            // load config
        }

        public void Stop()
        {
        }

        public bool IsAuthorizedToExecuteCommands(string userEmail)
        {
            return _isAuthorized;
        }

        public void GrantAccess(string userEmail)
        {
            _isAuthorized = true;
        }

        public void DenyAccess(string userEmail)
        {
            _isAuthorized = false;
        }
    }
}
