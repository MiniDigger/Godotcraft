using System;
using Godotcraft.scripts.objects.console.type;
using String = System.String;

namespace Godotcraft.scripts.objects.console.argument {
public class Argument {

	public string _name{ get; }
	public BaseType _type{ get; }
	public string _description { get; }
	public object _originalValue { get; private set; }

	public Argument(string name, BaseType type, string description = null) {
		_name = name;
		_type = type;
		_description = description;
	}

	public object getValue() {
		return _type._normalizedValue;
	}

	public BaseType.CHECK setValue(String value) {
		_originalValue = value;
		BaseType.CHECK check = _type.check(value);
		if (check == BaseType.CHECK.OK) {
			_type.normalized(value);
		}

		return check;
	}

	public String describe() {
		String argumentName = "";
		if (_name != null) {
			argumentName += _name + ":";
		}

		argumentName += _type.ToString();

		return argumentName;
	}
}
}