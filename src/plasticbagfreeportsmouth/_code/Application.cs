using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plasticbagfreeportsmouth {
    public static class Application {
        private static string _smtpServer, _smtpUsername, _smtpPassword, _smtpFrom, _takethepledge_to;
        public static void LoadFromConfig(IConfiguration Configuration) {
            _smtpServer = Configuration.Get("smtp:server");
            _smtpUsername = Configuration.Get("smtp:username");
            _smtpPassword = Configuration.Get("smtp:password");
            _smtpFrom = Configuration.Get("smtp:from");
            _takethepledge_to = Configuration.Get("takethepledge:form:email:to");
        }
        public static class Smtp {
            public static string Username { get { return _smtpUsername; } }
            public static string Password { get { return _smtpPassword; } }
            public static string From { get { return _smtpFrom; } }
            public static string Server { get { return _smtpServer; } }
        }
        public static class TakeThePledge {
            public static class Form {
                public static string EmailTo { get { return _takethepledge_to; } }
            }
        }
    }
}
