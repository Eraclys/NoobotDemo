using System;
using Noobot.Core;

namespace NoobotDemo
{
    public sealed class NoobotHost
    {
        private readonly INoobotCore _noobotCore;

        public NoobotHost(INoobotCore noobotCore)
        {
            _noobotCore = noobotCore;
        }

        public void Start()
        {
            Console.WriteLine("Connecting...");

            _noobotCore
                .Connect()
                .ContinueWith(task =>
                {
                    if (!task.IsCompleted || task.IsFaulted)
                    {
                        Console.WriteLine($"Error connecting to Slack: {task.Exception}");
                    }
                })
                .Wait();
        }

        public void Stop()
        {
            Console.WriteLine("Disconnecting...");
            _noobotCore?.Disconnect();
        }
    }
}
