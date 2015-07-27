using System;

namespace Site {
	public abstract class Page {
		public abstract string Description { get; }
		public abstract string Header { get; }
		public abstract string Key { get; }
		public abstract string Path { get; }
		public abstract string Title { get; }
		public abstract string TitleNav { get; }
		public abstract Func<string> Content { get; }
	}
}