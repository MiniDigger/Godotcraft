using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.objects.console.misc {
public class History {
	private Queue<String> queue;
	private int maxLength = -1;

	public History(int maxLength) {
		queue = new Queue<string>();
		this.maxLength = maxLength;
	}

	public void printAll() {
		int i = 1;
		foreach (string command in queue) {
			Console.instance.writeLine(
				"[b]" + i + ".[/b] [color=#ffff66][url=" +
				command + "]" + command + "[/url][/color]"
			);
			i += 1;
		}
	}

	public void push(string input) {
		queue.Enqueue(input);
	}

	public string next() {
		throw new NotImplementedException();
	}

	public void previous() {
		throw new NotImplementedException();
	}

	public string current() {
		throw new NotImplementedException();
	}
}
}