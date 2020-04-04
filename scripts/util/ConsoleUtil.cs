using System;
using Godot;

namespace Godotcraft.scripts.util {
public class ConsoleUtil {

	public static void writeToConsole(Node console, String message) {
		console.Call("writeLine",  message);
	}
	
	// public static void addCommand(Node console, String commandName, Node target, String targetName, String description, String argument) {
	// 	GD.Print("Register command");
	// 	console = console.Call("addCommand", commandName, target, targetName) as Node;
	// 	console = console?.Call("setDescription", description) as Node;
	// 	console = console?.Call("addArgument", argument, typeof(String)) as Node;
	// 	console?.Call("register");
	// 	GD.Print("Done");
	// }
	
	public static void addCommand(Node console, String commandName, Node target, String targetName, String description, String argument) {
		GD.Print("Register command");
		String command = "{\n  \"description\": " + description + ",\n  \"target\": [" + target + ", " + targetName + "]\n}";
		console.Call("register", commandName, command);
	}
}
}