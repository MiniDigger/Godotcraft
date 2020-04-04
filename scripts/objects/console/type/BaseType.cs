using System;
using System.Text.RegularExpressions;

namespace Godotcraft.scripts.objects.console.type {
public abstract class BaseType {
	public enum CHECK
	{
		OK,
		FAILED,
		CANCELED
	}

	public string _name { get; }
	public string _rematch { get; }
	public object _normalizedValue { get; set; }

	public string regex { get; }

	protected BaseType(string name) {
		_name = name;
	}

	public virtual CHECK check(object originalValue) {
		return recheck(regex, originalValue);
	}

	public CHECK recheck(string regex, object value) {
		if (regex != null) {
			_rematch = regex.search(value);

			if (_rematch != null) {
				return CHECK.OK;
			}
		}

		return CHECK.FAILED;
	}

	public abstract void normalized(object originalValue);
	
	public object getNormalizedValue() {
		return _normalizedValue;
	}

	public override string ToString() {
		return _name;
	}
}
}