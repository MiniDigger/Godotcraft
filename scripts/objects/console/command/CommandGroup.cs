using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Godotcraft.scripts.objects.console.command {
public class CommandGroup {
	public String _name { get; }
	public Dictionary<String, CommandGroup> _groups { get; }
	public Dictionary<String, Command> _commands { get; }

	public CommandGroup(String name) {
		_name = name;
		_groups = new Dictionary<String, CommandGroup>();
		_commands = new Dictionary<String, Command>();
	}

	public CommandGroup getGroup(List<String> nameParts, bool create = false) {
		if (nameParts.Count > 0) {
			String firstNamePart = nameParts[0];
			nameParts.Remove(firstNamePart);
			if (!_groups.ContainsKey(firstNamePart)) {
				if (create) {
					_groups[firstNamePart] = new CommandGroup(firstNamePart);
				}
				else {
					String found = null;
					int foundCount = 0;
					foreach (var groupName in _groups.Keys.Where(groupName => _groups[groupName]._name.BeginsWith(firstNamePart))) {
						found = groupName;
						foundCount += 1;
					}

					if (foundCount == 1) {
						firstNamePart = found;
					}
					else {
						Console.Log.error("CommandGroup: _getGroup: TODO: error"); // TODO: Change to proper error desc
						return null;
					}
				}
			}

			if (nameParts.Count > 1) {
				return _groups[firstNamePart].getGroup(nameParts);
			}

			return _groups[firstNamePart];
		}

		return null;
	}

	public Command getCommand(String name, List<object> parameters, bool register = false) {
		if (name == null) {
			return null;
		}
		List<String> nameParts = name.Split(".", false).ToList();

		if (nameParts.Count > 0) {
			String lastNamePart = nameParts[nameParts.Count - 1];
			CommandGroup group = getGroup(nameParts, register);  // TODO upstream this is just this
			if (nameParts.Count > 1) {
				nameParts.RemoveAt(nameParts.Count - 1);
				group = getGroup(nameParts, register);
			}

			if (group != null) {
				if (register && !group._commands.ContainsKey(lastNamePart)) {
					Command command = CommandBuilder.buildDeprecated(lastNamePart, parameters);

					group._commands[lastNamePart] = command;
					Console.Log.info("Successfully registered new command [b]`" + name + "`[/b].");
				}

				if (group._commands.ContainsKey(lastNamePart)) {
					Console.Log.debug("Successfully found existing command [b]`' + name + '`[/b].");
					return group._commands[lastNamePart];
				}

				if (Console.instance.submitAutocomplete) {
					Command found = null;
					int foundCount = 0;
					foreach (var commandName in group._commands.Keys.Where(commandName => group._commands[commandName]._name.BeginsWith(lastNamePart))) {
						found = group._commands[commandName];
						foundCount += 1;
					}

					if (foundCount == 1) {
						return found;
					}

					Console.Log.error("CommandGroup: _getCommand: Unable to provide with proper autocomplete.");
				}
			}
		}

		return null;
	}

	public bool registerCommand(String name, List<object> parameters) {
		return getCommand(name, parameters, true) != null;
	}
	
	public bool unregisterCommand(String name) {
		List<String> nameParts = name.Split(".", false).ToList();

		if (nameParts.Count > 0) {
			String lastNamePart = nameParts[nameParts.Count - 1];
			CommandGroup group = this;
			if (nameParts.Count > 1) {
				nameParts.RemoveAt(nameParts.Count - 1);
				group = getGroup(nameParts);
			}

			if (group != null) {
				if (group._commands.ContainsKey(lastNamePart)) {
					group._commands.Remove(lastNamePart);
					return true;
				} else {
					String found = null;
					int foundCount = 0;
					foreach (var commandName in group._commands.Keys.Where(commandName => group._commands[commandName]._name.BeginsWith(lastNamePart))) {
						found = commandName;
						foundCount += 1;
					}

					if (foundCount == 1 && found != null) {
						group._commands.Remove(found);
						return true;
					}

					Console.Log.error("CommandGroup: unregisterCommand:TODO: error"); // TODO: Change to proper error desc
				}
			}
		}

		return false;
	}

	public void printAll() {
		foreach (Command command in _commands.Values) {
			command.describe();
		}

		foreach (CommandGroup group in _groups.Values) {
			group.printAll();
		}
	}
}
}