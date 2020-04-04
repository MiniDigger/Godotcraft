using System;
using System.Collections.Generic;
using Godotcraft.scripts.objects.console.argument;
using Godotcraft.scripts.objects.console.type;
using String = System.String;

namespace Godotcraft.scripts.objects.console.command {
public class Command {
	public String _name { get; }
	public Action<List<object>> _target { get; }
	public List<Argument> _arguments { get; }
	public String _description { get; }

	public Command(string name, Action<List<object>> target, List<Argument> arguments, string description = null) {
		_name = name;
		_target = target;
		_arguments = arguments;
		_description = description;
	}

	public void execute(List<String> inArgs) {
		List<object> args = new List<object>();
		BaseType.CHECK argAssig;

		var i = 0;
		while (i < _arguments.Count && i < inArgs.Count) {
			argAssig = _arguments[i].setValue(inArgs[i]);

			switch (argAssig) {
				case BaseType.CHECK.FAILED:
					Console.Log.warn("Expected " + _arguments[i]._type + " as #" + (i + 1) + " argument.");
					return;
				case BaseType.CHECK.CANCELED:
					return;
				case BaseType.CHECK.OK:
					args.Add(_arguments[i].getValue());
					i += 1;
					break;
			}
		}

		// Execute command
		_target.Invoke(args);
	}

	public void describe() {
		Console.instance.write("[color=#ffff66][url=" + _name + "]" + _name + "[/url][/color]");

		foreach (var argument in _arguments) {
			Console.instance.write(" [color=#88ffff]" + argument.describe() + "[/color]");
		}

		if (_description != null) {
			Console.instance.write(" - " + _description);
		}

		Console.instance.writeLine();
	}
}
}