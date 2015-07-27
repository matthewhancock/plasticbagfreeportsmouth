using System;
using System.Collections.Generic;

namespace Site {
    public class Form {
        //private Section[] _sections;
        //private Field[] _fields;

        protected Form(Field[] Fields) {
        }



        public abstract class Field {
            private string _key, _displayName, _cssClass;
            private bool _required;
            private Dictionary<string, string> _attributes;

            public Field(string Key, string DisplayName, bool Required = false, string CssClass = null, Dictionary<string, string> Attributes = null) {
                _key = Key;
                _displayName = DisplayName;
                _cssClass = CssClass;
                _required = Required;
                _attributes = Attributes;
            }

            public string Key { get { return _key; } }
            public string DisplayName { get { return _displayName; } }
            public string CssClass { get { return _cssClass; } }
            public bool Required { get { return _required; } }
            public Dictionary<string, string> Attributes { get { return _attributes; } }

            public abstract FieldType Type { get; }
            public abstract string Output();

            public enum FieldType { Hidden = -1, Textbox = 0, TextArea = 1, Dropdown = 2 }


            public class Textbox : Field {
                private string _tag;
                public Textbox(string Key, string DisplayName, bool Required = false, string CssClass = null, Dictionary<string, string> Attributes = null, string Tag = "text") : base(Key, DisplayName, Required, CssClass, Attributes) {
                    _tag = Tag;
                }

                public override FieldType Type { get { return FieldType.Textbox; } }
                public override string Output() {
                    string s = "<div class=\"description\">" + DisplayName + "</div><input type=\"" + _tag + "\" data-key=\"" + Key + "\"";
                    if (Required) {
                        s += " data-required=\"true\"";
                    }
                    if (CssClass != null) {
                        s += " class=\"" + CssClass + "\"";
                    }
                    if (Attributes != null) {
                        foreach (var a in Attributes) {
                            s += " " + a.Key + "=\"" + a.Value + "\"";
                        }
                    }
                    return s + "/>";
                }
            }
            public class Textarea : Field {
                public Textarea(string Key, string DisplayName, bool Required = false, string CssClass = null, Dictionary<string, string> Attributes = null) : base(Key, DisplayName, Required, CssClass, Attributes) { }

                public override FieldType Type { get { return FieldType.Textbox; } }
                public override string Output() {
                    string s = "<div class=\"description\">" + DisplayName + "</div><textarea data-key=\"" + Key + "\"";
                    if (Required) {
                        s += " data-required=\"true\"";
                    }
                    if (CssClass != null) {
                        s += " class=\"" + CssClass + "\"";
                    }
                    if (Attributes != null) {
                        foreach (var a in Attributes) {
                            s += " " + a.Key + "=\"" + a.Value + "\"";
                        }
                    }
                    return s + "></textarea>";
                }
            }
        }
        //public abstract class Section {
        //	private Field[] _fields;
        //	private string _name;

        //	public Section(Field[] Fields, string DisplayName = null) {
        //		_fields = Fields;
        //		_name = DisplayName;
        //	}
        //}
    }
}