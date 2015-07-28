window.onpopstate = function (event) { OutputState(event.state); };

var v = (function (v) {
	v.email = function (e) {
		return /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/.test(e);
	}

	return v;
})(v ? v : {});

var util = (function (util) {
	// showing/hiding CSS
	util.hide = function (elementIDs) {
		if (Array.isArray(elementIDs)) {
			for (var i = 0, l = elementIDs.length; i < l; i++) {
				var e = util.get(elementIDs[i]);
				if (e) { e.classList.add("hide") }
			}
		} else {
			var e = util.get(elementIDs);
			if (e) { e.classList.add("hide") }
		}
	}
	util.unhide = function (elementIDs) {
		if (Array.isArray(elementIDs)) {
			for (var i = 0, l = elementIDs.length; i < l; i++) {
				var e = util.get(elementIDs[i]);
				if (e) { e.classList.remove("hide") }
			}
		} else {
			var e = util.get(elementIDs);
			if (e) { e.classList.remove("hide") }
		}
	}
	// creating DOM elements
	util.create = function (tagName, cssClass) {
		var e = document.createElement(tagName);
		if (cssClass != null) {
			e.setAttribute("class", cssClass)
		}
		return e;
	}
	// getting DOM elements
	util.get = function (elementID) {
		return document.getElementById(elementID);
	}

	// STRUCTURES //
	util.callback = function () {
		var f = [];
		var fst = null;
		this.add = function (callback) {
			f[f.length] = callback;
		};
		this.complete = function (lastCallback) {
			if (fst != null) {
				fst();
			}
			var len = f.length;
			if (len > 0) {
				for (var i = 0; i < len; i++) {
					if (f[i] != null) {
						f[i]();
					}
				}
			}
			if (lastCallback && (typeof lastCallback === 'function')) {
				lastCallback();
			}
		};
		this.first = function (firstCallback) {
			fst = firstCallback;
		};
	};
	return util;
})(util ? util : {});

var lastSelected;
var navigating = false;

function link(linkDOM) {
	if (history.pushState) {
		if (lastSelected == null || linkDOM.id !== lastSelected.id) { // don't do anything if clicking same link...
			if (!navigating) {
				navigating = true;

				var r = new XMLHttpRequest();
				r.addEventListener("load", function (response) {
					if (r.readyState == 4) {
						var d = JSON.parse(r.responseText);
						var state = { href: linkDOM.href, title: d.title, hrefid: linkDOM.id, content: d.content, header: d.header, key: d.key };
						for (p in linkDOM.dataset) {
							state[p] = linkDOM.dataset[p];
						}
						history.pushState(state, state.title, state.href);
						OutputState(state, linkDOM);
						navigating = false;
					}
				});
				r.addEventListener("error", function (response) {
				}, false);
				r.open('GET', "/json/" + linkDOM.dataset.page);
				r.send(null);
			}
		}
		return false;
	} else {
		return true;
	}
}

var nav = document.getElementById('n');
function OutputState(state, linkDOM) {
	if (state) {
	    OutputContent(state.content, state.title, state.header);
	    if (!nav) { nav = document.getElementById('n'); } // SVG slowing down DOM parsing in Chrome resulting in nav being null on initial load, despite existing.
		nav.dataset['key'] = state.key;
	}
}
function OutputContent(content, title, header) {
	if (title) {
		document.title = title;
	}
	var c = document.getElementById('content');
	c.innerHTML = content;
}


function a(fid, btn) {
	btn.disabled = true;
	var btnDisabled = true;
	var f = util.get(fid); var e = f.querySelectorAll('input, select, div[contenteditable], textarea'); var ok = true; var fd = new FormData();
	for (var i = 0, l = e.length; i < l; i++) {
		var ei = e[i];
		if (ei.dataset.required && ei.value === '') {
			ok = false;
			ei.classList.add("error");
			if (ei.onkeyup == null) {
			    ei.onkeyup = function (e) { if (this.value !== '') { this.classList.remove("error"); if (btnDisabled) { btn.disabled = false; btnDisabled = false } } };
			}
		} else {
		    ei.classList.remove("error");
		}

		if (ei.type === 'email') {
		    if (ei.value !== '' && !v.email(ei.value)) {
		        ei.classList.add("error");
				ok = false;
			}
		}
		var key = ei.dataset.key;
		if (key != null) {
			if (ei.type) {
				if (ei.type === 'checkbox') {
					fd.append(key, ei.checked);
				} else {
					fd.append(key, ei.value);
				}
			} else if (ei.value) { // textarea
				fd.append(key, ei.value);
			} else if (ei.contentEditable) {
				fd.append(key, ei.innerHTML);
			}
		}
	}
	var e = util.get(fid + '_error');
	if (ok) {
		e.classList.add("hide");
		// submit...
		var r = new XMLHttpRequest();
		r.addEventListener("load", function (response) {
			try {
				var o = JSON.parse(this.responseText);
				if (o.ok) {
					if (o.message) {
						e.innerHTML = o.message;
						e.classList.remove("hide");
						if (o.reenable) { btn.disabled = false; }
					} else if (o.form) {
						var tempDiv = util.create('div');
						tempDiv.innerHTML = o.form;
						f.parentNode.replaceChild(tempDiv.firstChild, f);
					} else if (o.script) {
						f.innerHTML = "";
						var script = document.createElement('script');
						script.text = o.script;
						f.appendChild(script);
					} else if (o.substitute) {
						var s = util.get(o.substitute.id);
						if (s) { s.innerHTML = o.substitute.value; }
						if (o.reenable) { btn.disabled = false; }
					} else if (o.push) {
						var state = o.push.state;
						history.pushState(state, state.title, state.href);
						OutputState(state, null);
					} else {
						f.innerHTML = "OK";
					}
				} else {
					e.innerHTML = o.message ? o.message : "Unknown Error...";
					e.classList.remove("hide");
				}
			} catch (ex) {
				e.innerHTML = "Unknown Server Error...";
				e.classList.remove("hide");
				console.log(ex.message);
			}
		}, false);
		r.addEventListener("error", function (response) {
			e.innerHTML = "Error....";
			e.classList.remove("hide");
		}, false);
		r.open('POST', "/" + f.dataset.action);
		r.send(fd);
	} else {
		e.innerHTML = "Please fill required fields.";
		btn.disabled = false;
		btnDisabled = false;
		e.classList.remove("hide");
	}
};