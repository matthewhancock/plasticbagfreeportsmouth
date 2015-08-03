using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plasticbagfreeportsmouth {
    public static class Application {
        private static string _queueName, _queueKey, _takethepledge_to;
        public static void LoadFromConfig(IConfiguration Configuration) {
            _queueName = Configuration.Get("queue:name");
            _queueKey = Configuration.Get("queue:key");
            _takethepledge_to = Configuration.Get("takethepledge:form:email:to");
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
