using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using System;

namespace NoobotDemo.Core
{
    public class ValidHandle : IValidHandle
    {
        private readonly Predicate<string> _isMatch;

        public ValidHandle(string handleHelpText, Predicate<string> isMatch)
        {
            HandleHelpText = handleHelpText;
            _isMatch = isMatch;
        }

        public bool IsMatch(string message)
        {
            return _isMatch(message);
        }

        public string HandleHelpText { get; }


        public static IValidHandle[] For(string handleHelpText, Predicate<string> isMatch)
        {
            return new IValidHandle[] {new ValidHandle(handleHelpText, isMatch)};
        }
    }
}
