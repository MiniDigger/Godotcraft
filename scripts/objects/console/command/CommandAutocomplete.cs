using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.objects.console.command {
public class CommandAutocomplete {

	private String _filter = null;
	private List<String> _filtered = new List<string>();
	private int _current = -1;

	public void filter(String filter) {
		if (filter.Equals(_filter)) {
			return;
		}

		// TODO implement autocomplete
		// Console.instance._Commands._commands;
	}

	public String next() {
		if (_filtered.Count > 0) {
			if (_current == _filtered.Count - 1) {
				_current = -1;
			}

			_current += 1;
			return _filtered[_current];
		}

		reset();
		return null;
	}

	public void reset() {
		_filter = null;
		_filtered = new List<string>();
		_current = -1;
	}
}
}