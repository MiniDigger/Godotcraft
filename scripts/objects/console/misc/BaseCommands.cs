using System;
using Godot;
using Godotcraft.scripts.objects.console.command;
using Godotcraft.scripts.objects.console.type;
using String = Godotcraft.scripts.objects.console.type.String;

namespace Godotcraft.scripts.objects.console.misc {
public class BaseCommands {
	public static void init() {
		Console.instance.addCommand("echo", text => Console.instance.write(text.Count > 0 ? (string)text[0] : null))
			.setDescription("Prints a string.")
			.addArgument("text", typeof(String))
			.register();

		Console.instance.addCommand("history", _ => Console.History.printAll())
			.setDescription("Print all previous commands used during the session.")
			.register();

		Console.instance.addCommand("commands", _ => Console.instance._rootGroup.printAll())
			.setDescription("Lists all available commands.")
			.register();

		Console.instance.addCommand("help", command => help(command.Count > 0 ? (string)command[0] : null))
			.setDescription("Outputs usage instructions.")
			.addArgument("command", typeof(String))
			.register();

		Console.instance.addCommand("quit", _ => quit())
			.setDescription("Exit application.")
			.register();

		Console.instance.addCommand("clear", _ => Console.instance.clear())
			.setDescription("Clear the terminal.")
			.register();

		Console.instance.addCommand("version", _ => version())
			.setDescription("Shows engine version.")
			.register();

		Console.instance.addCommand("fps_max", fps => {
				if (fps.Count > 0) {
					Engine.TargetFps = (int) fps[0];
				}
			})
			.setDescription("The maximal framerate at which the application can run.")
			.addArgument("fps", typeof(Int))
			.register();
	}

	public static void help(string command = null) {
		if (command != null) {
			Command cmd = Console.instance.getCommand(command);

			if (cmd == null) {
				Console.Log.warn("No such command.");
				return;
			}

			cmd.describe();
		}
		else {
			Console.instance.writeLine(
				"Type [color=#ffff66][url=help]help[/url] <command-name>[/color] show information about command.\n" +
				"Type [color=#ffff66][url=commands]commands[/url][/color] to get a list of all commands.\n" +
				"Type [color=#ffff66][url=quit]quit[/url][/color] to exit the application.");
		}
	}

	public static void version() {
		Console.instance.writeLine(Engine.GetVersionInfo().ToString());
	}

	public static void quit() {
		Console.Log.warn("Quitting application...");
		Console.instance.GetTree().Quit();
	}
}
}