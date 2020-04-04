using System;
using System.Text.RegularExpressions;
using Godot;

namespace Godotcraft.scripts.objects.console.type {
public abstract class BaseType {
	public enum CHECK
	{
		OK,
		FAILED,
		CANCELED
	}

	public string _name { get; }
	public RegExMatch _rematch { get; private set; }
	public object _normalizedValue { get; set; }

	public RegEx regex;


	protected BaseType(string name, string pattern) {
		_name = name;
		if (pattern != null) {
			regex = new RegEx();
			regex.Compile(pattern);
		} 
	}

	public virtual CHECK check(string originalValue) {
		return recheck(regex, originalValue);
	}

	public CHECK recheck(RegEx regex, string value) {
		if (regex != null) {
			_rematch = regex.Search(value);

			if (_rematch != null) {
				return CHECK.OK;
			}
		}

		return CHECK.FAILED;
	}

	public abstract void normalized(string originalValue);
	
	public object getNormalizedValue() {
		return _normalizedValue;
	}

	public override string ToString() {
		return _name;
	}
}
}