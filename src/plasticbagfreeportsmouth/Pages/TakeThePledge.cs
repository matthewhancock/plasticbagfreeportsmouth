using System;
using System.Collections.Generic;

namespace plasticbagfreeportsmouth.Pages {
	public class TakeThePledge : Site.Page {

		public static string OutputPage() {
			var token = Guid.NewGuid().ToString("N");
			return $"<div class=\"tac\">All businesses, including non-profits, are welcome to take the pledge.<br /><br /><b>My business pledges to not issue single-use plastic shopping bags.</b><br /><br /><div id=\"{Forms.TakeThePledge.HtmlID.FormContainer}\"><div id=\"f_{token}\" data-action=\"{Forms.TakeThePledge.Action.Form}\" class=\"form ib\">" + 
                new Site.Form.Field.Textbox(Forms.TakeThePledge.Keys.BusinessName, "Business Name", true, null, new Dictionary<string, string>() { { "autofocus", "" } }).Output() +
                new Site.Form.Field.Textarea(Forms.TakeThePledge.Keys.Address, "Address", true).Output() +
                new Site.Form.Field.Textbox(Forms.TakeThePledge.Keys.OwnerManagerName, "Owner/Manager's Name", true).Output() +
                new Site.Form.Field.Textbox(Forms.TakeThePledge.Keys.PhoneNumber, "Phone Number", true).Output() +
                new Site.Form.Field.Textbox(Forms.TakeThePledge.Keys.Email, "Email Address", true, null, null, "email").Output() +
                new Site.Form.Field.Textbox(Forms.TakeThePledge.Keys.Website, "Website [Optional]*").Output() +
                "<br /><br /><div id=\"f_" + token + "_error\" class=\"error hide\"></div><input type=\"button\" onclick=\"a('f_" + token + "',this)\" value=\"Submit\" /></div></div><br />" +
                "Upon confirmation of the information provided above, you will receive a packet that includes the Plastic Bag Free Portsmouth window sticker, permission to use the Plastic Bag Free Portsmouth logo on marketing materials, and your business' name listed and hyperlinked* on this website.<br /><br />* Only businesses that provide a link to their homepage will be hyperlinked.</div>";
        }

		public override Func<string> Content {
			get {
				return OutputPage;
			}
		}

		public override string Description {
			get {
				return "How to get involved and help elect Democrats in Portsmouth, New Hampshire.";
			}
		}

		public override string Key {
			get {
				return "take-the-pledge";
			}
		}
		public override string Path {
			get {
				return "Take-The-Pledge";
			}
		}
		public override string Header {
			get {
				return "Take The Pledge";
			}
		}

		public override string Title {
			get {
				return "Plastic Bag Free Portsmouth - " + Header;
			}
		}

		public override string TitleNav {
			get {
				return "Take The Pledge";
			}
		}
	}
}