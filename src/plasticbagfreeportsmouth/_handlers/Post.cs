using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace plasticbagfreeportsmouth.Handlers {
    public class Post {
        public static async Task ProcessRequestAsync(HttpContext Context, string Action) {
            var form = Context.Request.Form;
            Context.Response.ContentType = "text/javascript";

            switch (Action) {
                case Forms.TakeThePledge.Action.Form:
                    var businessName = form[Forms.TakeThePledge.Keys.BusinessName];
                    var address = form[Forms.TakeThePledge.Keys.Address];
                    var manager = form[Forms.TakeThePledge.Keys.OwnerManagerName];
                    var phoneNumber = form[Forms.TakeThePledge.Keys.PhoneNumber];
                    var email = form[Forms.TakeThePledge.Keys.Email];
                    var website = form[Forms.TakeThePledge.Keys.Website];

                    string message = $"Business Name: {businessName}\r\nAddress: {address}\r\nOwner/Manager: {manager}\r\nPhone Number: {phoneNumber}\r\nEmail: {email}\r\nWebsite: {website}";

                    var e = new System.Net.Mail.SmtpClient(Application.Smtp.Server);
                    e.EnableSsl = true;
                    e.Credentials = new System.Net.NetworkCredential(Application.Smtp.Username, Application.Smtp.Password);
                    try {
                        await e.SendMailAsync(new System.Net.Mail.MailMessage(Application.Smtp.From, Application.TakeThePledge.Form.EmailTo, "New Business Took Bag Free Portsmouth Pledge", message));
                        await Context.Response.WriteAsync(Response.Substitute(Forms.TakeThePledge.HtmlID.FormContainer, "<div class=\"tac green\">Thanks! Your information has been received and you'll be contacted shortly.</div>"));
                    } catch {
                        await Context.Response.WriteAsync(Response.Substitute(Forms.TakeThePledge.HtmlID.FormContainer, $"<div class=\"tac green\">Sorry, there was an error processing the form.</div>"));
                    }
                    break;
            }
        }


        public class Response {
            public static string Form(string Content) {
                return "{\"ok\":true,\"form\":\"" + Fix(Content) + "\"}";
            }
            public static string Substitute(string ID, string Value) {
                return "{\"ok\":true,\"reenable\":true,\"substitute\":{\"id\":\"" + ID + "\", \"value\":\"" + Fix(Value) + "\"}}";
            }
            public static string Push(string Href, string Title, string Content, string HrefID = null) {
                if (Href != null) {
                    return "{\"ok\":true,\"push\":{\"state\":{\"href\":\"" + Href + "\", \"title\":\"" + Fix(Title) + "\", \"content\":\"" + Fix(Content) + "\", \"hrefid\":\"" + Fix(HrefID) + "\"}}}";
                } else {
                    return "{\"ok\":true,\"push\":{\"state\":{\"href\":\"" + Href + "\", \"title\":\"" + Fix(Title) + "\", \"content\":\"" + Fix(Content) + "\"}}}";
                }
            }
            public static string OK(string Message) {
                return "{\"ok\":true,\"reenable\":true,\"message\":\"" + Fix(Message) + "\"}";
            }
            public static string Error(string Message) {
                return "{\"ok\":false,\"message\":\"" + Fix(Message) + "\"}";
            }
            public static string Fix(string s) {
                return Util.Json.Fix(s);
            }
        }
    }
}