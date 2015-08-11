using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace plasticbagfreeportsmouth.Pages {
    public class Supporters : Site.Page {

        private static string _output = null;
        public static string OutputPage() {
            if (_output == null) {
                var supporters = JsonConvert.DeserializeObject<List<Supporter>>(Util.File.LoadToString("Data/Supporters.json").Result);
                var sb = new System.Text.StringBuilder();
                sb.Append("<blockquote>As a business owner, I am excited to take the pledge. In addition to supporting local artists and fair trade within my business, I think it is important to be good stewards of the environment by not issuing single-use plastic shopping bags.<aside>Brie Delisi, Prelude, 65 Market Street</aside></blockquote>");
                sb.Append("<div class=\"tac\">");
                foreach (var i in supporters) {
                    sb.Append($"<h3><a href=\"{i.Url}\" target=\"_blank\">{i.Name}</a></h3>");
                }
                sb.Append("</div>");
                sb.Append("<blockquote>We have always encouraged our customers to use our recycled cardboard can flats for can purchases. Our customers feel good about using alternatives to plastic such as the cardboard flats. Sometimes, coming up with creative ideas for plastic bag alternatives not only helps the environment, but also educates customers and can cut costs as well.<aside>Dawn Price, The Natural Dog and Holistic Cat, 801 Islington Street</aside></blockquote>");
                _output = sb.ToString();
            }
            return _output;
        }

        public override Func<string> Content {
            get {
                return OutputPage;
            }
        }

        public override string Description {
            get {
                return "Supporters that have taken the Plastic Bag Free Portsmouth Pledge";
            }
        }

        public override string Key {
            get {
                return "supporters";
            }
        }
        public override string Path {
            get {
                return "Businesses";
            }
        }
        public override string Header {
            get {
                return "Businesses";
            }
        }

        public override string Title {
            get {
                return "Plastic Bag Free Portsmouth - " + Header;
            }
        }

        public override string TitleNav {
            get {
                return "Businesses";
            }
        }

        public class Supporter {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Address { get; set; }
        }
    }
}
