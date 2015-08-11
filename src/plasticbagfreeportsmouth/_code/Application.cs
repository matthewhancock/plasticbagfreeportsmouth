using Microsoft.Framework.Configuration;
using Microsoft.Framework.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plasticbagfreeportsmouth {
    public static class Application {
        private static string _queueName, _queueKey, _takethepledge_to;
        private static string _path;
        public static void LoadFromEnvironment(IApplicationEnvironment env) {
            _path = env.ApplicationBasePath;
        }
        public static void LoadFromConfig(IConfiguration Configuration) {
            _queueName = Configuration.Get("queue:name");
            _queueKey = Configuration.Get("queue:key");
            _takethepledge_to = Configuration.Get("takethepledge:form:email:to");
        }

        public static class Environment {
            public static string ApplicationBasePath { get { return _path; } }
        }

        public static class Queue {
            public static string Name { get { return _queueName; } }
            public static string Key { get { return _queueKey; } }
        }

        public static class TakeThePledge {
            public static class Form {
                public static string EmailTo { get { return _takethepledge_to; } }
            }
        }
    }
}
