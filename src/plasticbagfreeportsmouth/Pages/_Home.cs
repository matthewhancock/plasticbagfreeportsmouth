using System;

namespace plasticbagfreeportsmouth.Pages {
    public class Home : Site.Page {

        public static string OutputPage() {
            return "<b>The Plastic Bag Free Portsmouth Pledge</b> encourages businesses to voluntarily stop issuing single-use plastic shopping bags. This effort is a follow-up to the Rise Above Plastics (RAP) Coalition's successful education campaign that surrounded Portsmouth's plastic bag ordinance.*<br /><br />" +
                "<b>Taking the Pledge</b> is free for all businesses committed to not issuing traditional single-use plastic shopping bags. Businesses that take the pledge will receive a packet that includes the Plastic Bag Free Portsmouth window sticker, permission to use the Plastic Bag Free Portsmouth logo on their marketing materials, and their business' name listed and hyperlinked on this website.<br /><br />" +
                "<b>Raising Awareness</b> of the harm from single-use plastics is important. The City of Portsmouth does not recycle single-use plastic shopping bags, which end up being landfilled, incinerated, or due to their aerodynamics, pollute our landscape, waterways, and beaches. Instead of decomposing, plastics break down into smaller and smaller pieces that persist in the environment for hundreds of years. These plastic pieces are often mistaken as food and are digested by marine life.<br /><br />" +
                "<b>Reducing single-use plastic shopping bags</b> is the goal of the Plastic Bag Free Portsmouth Pledge. As individuals, businesses, and as a community, we all have a part to play in taking actions towards a more sustainable future.<br /><br />" +
                "* The Portsmouth City Council did not vote on the ordinance and therefore the RAP Coalition is seeking more specific enabling legislation at the State level.";
        }

        public override Func<string> Content {
            get {
                return OutputPage;
            }
        }

        public override string Description {
            get {
                return "The Plastic Bag Free Portsmouth Pledge encourages businesses to voluntarily stop issuing single-use plastic shopping bags. This effort is a follow-up to the Rise Above Plastics (RAP) Coalition's successful education campaign that surrounded Portsmouth's plastic bag ordinance.";
            }
        }

        public override string Key {
            get {
                return "home";
            }
        }
        public override string Path {
            get {
                return string.Empty;
            }
        }
        public override string Header {
            get {
                return "Plastic Bag Free Portsmouth";
            }
        }

        public override string Title {
            get {
                return "Plastic Bag Free Portsmouth";
            }
        }

        public override string TitleNav {
            get {
                return "Home";
            }
        }
    }
}
