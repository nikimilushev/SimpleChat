using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleChat.Server;
using SimpleChat.DI;

namespace SimpleChat.Test
{
    class ServiceRunner
    {
        Thread _workerThread;

        private EventWaitHandle _canExit = new AutoResetEvent(false);

        private EventWaitHandle _serverStarted;

        public ServiceRunner(EventWaitHandle serverStarted)
        {
            _serverStarted = serverStarted;
        }        

        public void StartService()
        {
            _workerThread = new Thread(() =>
                {
                    using (var serviceHost = new StructureMapServiceHost(typeof(ChatService)))
                    {
                        Console.Out.WriteLine("server is starting...");

                        serviceHost.Open();

                        this._serverStarted.Set();

                        Console.Out.WriteLine("server started");

                        this._canExit.WaitOne();

                        Console.Out.WriteLine("server is stopping...");

                        serviceHost.Close();

                        Console.Out.WriteLine("server stopped");
                    }
                }
            );
            _workerThread.Start();
        }

        public void StopService()
        {
            _canExit.Set();

            _workerThread.Join();
        }
    }
}
