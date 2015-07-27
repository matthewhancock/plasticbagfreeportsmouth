using System;

namespace Util.Http {
	public static class Headers {
		public const string Cache = "Cache-Control";

		public class Values {
			public static string[] Cache = { "public", "max-age=31557600" }; // 365.25 * 24 * 60 * 60 - one year in seconds
		}
	}
}