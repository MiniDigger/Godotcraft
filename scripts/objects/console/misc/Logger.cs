using System;

namespace Godotcraft.scripts.objects.console.misc {
public class Logger {
	public enum TYPE {
		DEBUG,
		INFO,
		WARNING,
		ERROR,
		NONE
	}

	private TYPE logLevel = TYPE.WARNING;

	public Logger log(String message, TYPE type = TYPE.INFO) {
		switch (type) {
			case TYPE.DEBUG:
				return debug(message);
			case TYPE.INFO:
				return info(message);
			case TYPE.WARNING:
				return warn(message);
			case TYPE.ERROR:
				return error(message);
			default:
				return this;
		}
	}

	public Logger debug(String message) {
		if (logLevel <= TYPE.DEBUG) {
			Console.instance.writeLine("[color=green][DEBUG][/color] " + message);
		}

		return this;
	}
	
	public Logger info(String message) {
		if (logLevel <= TYPE.INFO) {
			Console.instance.writeLine("[color=blue][INFO][/color] " + message);
		}

		return this;
	}
	
	public Logger warn(String message) {
		if (logLevel <= TYPE.WARNING) {
			Console.instance.writeLine("[color=yellow][WARNING][/color] " + message);
		}

		return this;
	}
	
	public Logger error(String message) {
		if (logLevel <= TYPE.ERROR) {
			Console.instance.writeLine("[color=red][ERROR][/color] " + message);
		}

		return this;
	}
}
}