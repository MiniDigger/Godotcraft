using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godotcraft.scripts.objects.console.argument;

namespace Godotcraft.scripts.objects.console.command {
public class CommandBuilder {
	private String _name;
	private Action<object> _target;
	private List<Argument> _arguments;
	private String _description;
	private CommandGroup _commandGroup;

	public CommandBuilder(CommandGroup commandGroup, string name, Action<object> target) {
		_name = name;
		_target = target;
		_commandGroup = commandGroup;
		_arguments = new List<Argument>();
		_description = null;
	}

	public CommandBuilder addArgument(String name, Type type = null, String description = null) {
		_arguments.Add(ArgumentFactory.create(name, type, description));
		return this;
	}

	public CommandBuilder setDescription(String description = null) {
		_description = description;
		return this;
	}

	public void register() {
		Command command = new Command(_name, _target, _arguments, _description);
		List<String> nameParts = _name.Split(".", false).ToList();
		String lastNamePart = nameParts[nameParts.Count - 1];
		CommandGroup group = _commandGroup;
		if (nameParts.Count > 0) {
			group = _commandGroup.getGroup(nameParts, true);
		}

		group._commands[lastNamePart] = command;
	}
	
	//TODO stuff is missing here

	public static Command buildDeprecated(String name, List<object> parameters) {
		throw new NotImplementedException();
	}
}
}