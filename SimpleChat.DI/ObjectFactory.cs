using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using StructureMap.Graph;
using SimpleChat.Model;
using StructureMap.Graph.Scanning;
using SimpleChat.SQL;
using SimpleChat.ChatServer;

namespace SimpleChat.DI
{
    public static class ObjectFactory
    {
        private static Lazy<Container> _containerBuilder=
                new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            var container = new Container(_ =>
            {
                _.For<AuditRepository>().Singleton().Use<SqlAuditRepository>();
                _.For<ChatBase>().Singleton().Use<AuditingChat>();
            });

            return container;
        }

        public static void ReconfigureContainer(Func<Container> newContainer)
        {
            _containerBuilder = new Lazy<Container>(newContainer, LazyThreadSafetyMode.ExecutionAndPublication);
        }

    }    
}