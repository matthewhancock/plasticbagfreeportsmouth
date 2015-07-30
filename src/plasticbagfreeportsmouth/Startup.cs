using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Util.Http;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Configuration;
using Util.AspNet;

namespace plasticbagfreeportsmouth {
    public class Startup {
        private string _path;
        private IConfiguration _config;

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env) {
            _path = env.ApplicationBasePath;

            // load config
            _config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            // load local config file if no environmentvariables
            var envn = _config.Get("env") ?? "local";
            if (envn == "local") {
                _config = new ConfigurationBuilder().AddJsonFile(_path + "/config.local.json").Build();
            }
            Application.LoadFromConfig(_config);

            app.ForceHttps();
            app.Run(ProcessRequestAsync);
        }

        public async Task ProcessRequestAsync(HttpContext Context) {
            var rq = Context.Request;
            var rs = Context.Response;

            var path = rq.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var pathc = path.Length;

            if (rq.Method == "POST") {
                if (pathc == 1) {
                    await Handlers.Post.ProcessRequestAsync(Context, path[0]);
                }
            } else {
                if (pathc > 0) {
                    switch (path[0]) {
                        case "css":
                            await Css(rs);
                            break;
                        case "js":
                            await Javascript(rs);
                            break;
                        case "image":
                            await Image(rs, path[1]);
                            break;
                        case "json":
                            if (pathc == 2) {
                                await OutputPageJson(rs, path[1]);
                            } else if (pathc > 2) {
                                await OutputPageJson(rs, path[1]);
                            } else {
                                await Error.FileNotFound(rs);
                            }
                            break;
                        default:
                            if (pathc == 1) {
                                await OutputPage(rs, path[0]);
                            } else if (pathc > 1) {
                                await OutputPage(rs, path[0], path.Skip(1).ToArray());
                            } else {
                                await Error.FileNotFound(rs);
                            }
                            break;
                    }
                } else {
                    await OutputPage(rs, string.Empty);
                }
            }
        }

        private static class Pages {
            public static Site.Page Home = new plasticbagfreeportsmouth.Pages.Home();
            public static Site.Page TakeThePledge = new plasticbagfreeportsmouth.Pages.TakeThePledge();
        }

        private static string tags = (new Util.Html.Head.Tag("link", new Dictionary<string, string> { { "rel", "stylesheet" }, { "type", "text/css" }, { "href", "/css/" } })).Output() +
            (new Util.Html.Head.Tag("link", new Dictionary<string, string> { { "rel", "stylesheet" }, { "type", "text/css" }, { "href", "//cloud.typography.com/607958/668628/css/fonts.css" } })).Output() +
            (new Util.Html.Head.Tag.Javascript("/js/")).Output() + (new Util.Html.Head.Tag.Javascript("//platform.twitter.com/widgets.js")).Output() + (new Util.Html.Head.Tag.Javascript("//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=218352134886679")).Output();
        private static string body_start = $"<div id=\"c\"><header id=\"h\"><aside class=\"h-logo\">{Site.Svg.Sticker1}</aside><h1 id=\"h_h1\">Plastic Bag Free Portsmouth</h1><aside class=\"h-logo\">{Site.Svg.RiseAbovePlastics}</aside></header><nav id=\"n\" data-key=\"";
        private static string body_mid = $"\"><a id=\"link-{Pages.Home.Key}\" href=\"/{Pages.Home.Path}\" data-page=\"{Pages.Home.Key}\" onclick=\"return link(this)\">{Pages.Home.TitleNav}</a>" +
            $"<a id=\"link-{Pages.TakeThePledge.Key}\" href=\"/{Pages.TakeThePledge.Path}\" data-page=\"{Pages.TakeThePledge.Key}\" onclick=\"return link(this)\">{Pages.TakeThePledge.TitleNav}</a>" +
            "</nav><hr /><main id=\"m\"><section id=\"content\">";
        private static string body_end = $"</section><aside id=\"social\"><a href=\"https://twitter.com/BagFreePorts\" class=\"twitter-follow-button\" data-show-count=\"false\" data-size=\"large\" data-dnt=\"true\">Follow @BagFreePorts</a><div class=\"fb-like-box\" data-href=\"https://www.facebook.com/PlasticBagFreePortsmouth\" data-colorscheme=\"light\" data-show-faces=\"false\" data-header=\"false\" data-stream=\"false\" data-show-border=\"false\"></div></aside></main><footer id=\"f\">{Site.Svg.Sticker2}</footer></div>";
        private async Task OutputPage(HttpResponse Response, string Path, string[] Parameters = null) {
            Site.Page page = null;

            if (Path == Pages.Home.Path) {
                page = Pages.Home;
            } else if (Path == Pages.TakeThePledge.Path) {
                page = Pages.TakeThePledge;
            }

            if (page == null) {
                await Error.FileNotFound(Response);
            } else {
                Response.ContentType = "text/html";
                await Util.Html.WriteOutput(Response, page.Title, tags + "<meta name=\"description\" content=\"" + page.Description + "\">", body_start + page.Key + body_mid + page.Content.Invoke() + body_end);
            }
        }
        private async Task OutputPageJson(HttpResponse Response, string Key) {
            if (Key == Pages.Home.Key) {
                await Json(Response, Pages.Home);
            } else if (Key == Pages.TakeThePledge.Key) {
                await Json(Response, Pages.TakeThePledge);
            }
        }

        private static string _css, _javascript = null;
        private static Dictionary<string, byte[]> _images = new Dictionary<string, byte[]>();
        private async Task Css(HttpResponse Response) {
            Response.Headers.Add(Headers.Cache, Headers.Values.Cache);
            Response.ContentType = "text/css";

            if (_css == null) {
                _css = await Util.File.LoadToString(_path, "_files/css/this.css");
            }
            await Response.WriteAsync(_css);
        }
        private async Task Javascript(HttpResponse Response) {
            Response.Headers.Add(Headers.Cache, Headers.Values.Cache);
            Response.ContentType = "text/javascript";

            if (_javascript == null) {
                _javascript = await Util.File.LoadToString(_path, "_files/js/this.js");
            }
            await Response.WriteAsync(_javascript);
        }
        private async Task Image(HttpResponse Response, string Path) {
            Response.Headers.Add(Headers.Cache, Headers.Values.Cache);
            Response.ContentType = "image/jpeg";

            byte[] file = null;
            if (!_images.ContainsKey(Path)) {
                try {
                    file = await Util.File.LoadToBuffer(_path, $"_files/images/{Path}.jpg");
                    _images.Add(Path, file);
                } catch { }
            } else {
                file = _images[Path];
            }
            if (file != null) {
                await Response.Body.WriteAsync(file, 0, file.Length);
            } else {
                Response.StatusCode = 404;
                Response.Body.Close();
            }
        }
        private async Task Json(HttpResponse Response, Site.Page Page) {
            Response.ContentType = "text/javascript";
            await Response.WriteAsync(@"{""title"":""" + Util.Json.Fix(Page.Title) + @""",""description"":""" + Util.Json.Fix(Page.Description) + @""",""header"":""" + Util.Json.Fix(Page.Header) + @""",""key"":""" + Page.Key + @""",""content"":""" + Util.Json.Fix(Page.Content.Invoke()) + @"""}");
        }

        private static class Error {
            public static async Task FileNotFound(HttpResponse Response) {
                Response.StatusCode = 404;
                await Response.WriteAsync("File Not Found");
            }
        }
    }
}
