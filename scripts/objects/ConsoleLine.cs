using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;

namespace Godotcraft.scripts.objects {
public class ConsoleLine : LineEdit {
	public static readonly string COMMANDS_SEPARATOR = ";";

	public static readonly string RECOMMANDS_SEPARATOR = "(?<!\\\\)" + COMMANDS_SEPARATOR;

	public static readonly char COMMAND_PARTS_SEPARATOR = ' ';

	public static readonly List<string> QUOTES = new[]{"\"", "'"}.ToList();

	public static readonly List<string> SCREENERS = new[]{"\\/"}.ToList();

	private string _tmpUsrEnteredCmd;

	private string _currCmd;

	public override void _Ready() {
		SetProcessInput(true);
		Connect("text_entered", this, nameof(execute));
	}

	public override void _Input(InputEvent @event) {
		// Show next line in history
		if (Input.IsActionJustPressed(Actions.ui_up)) {
			_currCmd = Console.History.current();
			Console.History.previous();

			if (_tmpUsrEnteredCmd == null) {
				_tmpUsrEnteredCmd = Text;
			}
		}

		// Show previous line in history
		if (Input.IsActionJustPressed(Actions.ui_down)) {
			_currCmd = Console.History.next();

			if (_currCmd != null && _tmpUsrEnteredCmd != null) {
				_currCmd = _tmpUsrEnteredCmd;
				_tmpUsrEnteredCmd = null;
			}
		}

		// Autocomplete on TAB
		// TODO: Maybe later

		// Finish
		if (_currCmd != null) {
			Text = _currCmd;
			AcceptEvent();
			_currCmd = null;
		}
	}

	public void setText(string text, bool moveCaretToEnd = true) {
		Text = text;
		GrabFocus();

		if (moveCaretToEnd) {
			CaretPosition = text.Length;
		}
	}

	public void execute(string input) {
		Console.instance.writeLine("[color=#999999]$[/color] " + input);

		string[] rawCommands = Regex.Split(input, RECOMMANDS_SEPARATOR);

		var parsedCommands = parseCommands(rawCommands);

		foreach (var parsedCommand in parsedCommands) {
			var command = Console.instance.getCommand((String) parsedCommand["name"]);

			if (command != null) {
				Console.Log.debug("Executing `' + parsedCommand.command + '`.");
				command.execute((List<String>)parsedCommand["arguments"]);
				Console.History.push(input);
				Clear();
			}
		}
	}

	public List<Dictionary<string, object>> parseCommands(string[] rawCommands) {
		var resultCommands = new List<Dictionary<string, object>>();

		foreach (var rawCommand in rawCommands) {
			resultCommands.Add(parseCommand(rawCommand));
		}

		return resultCommands;
	}

	public Dictionary<string, object> parseCommand(string rawCommand) {
		string name = null;
		List<string> arguments = new List<string>();

		int beginning = 0;
		char openQuote = '-';
		bool isInsideQuotes = false;
		string substring = null;
		for (var i = 0; i < rawCommand.Length; i++) {
			// Quote
			if (QUOTES.Contains(rawCommand[i].ToString()) && i > 0 && !SCREENERS.Contains(rawCommand[i - 1].ToString())) {
				if (isInsideQuotes && rawCommand[i] == openQuote) {
					openQuote = '-';
					isInsideQuotes = false;
					substring = rawCommand.Substring(beginning, i - beginning);
					beginning = i + 1;
				}
				else if (!isInsideQuotes) {
					openQuote = rawCommand[i];
					isInsideQuotes = true;
					beginning += 1;
				}
			}

			// Separate arguments
			else if (rawCommand[i] == COMMAND_PARTS_SEPARATOR && !isInsideQuotes || i == rawCommand.Length - 1) {
				if (i == rawCommand.Length - 1) {
					substring = rawCommand.Substring(beginning, i - beginning + 1);
				}
				else {
					substring = rawCommand.Substring(beginning, i - beginning);
					beginning = i + 1;
				}
			}

			// Save separated argument
			if (substring != null && !substring.Empty()) {
				if (name == null) {
					name = substring;
				}
				else {
					arguments.Add(substring);
					substring = null;
				}
			}
		}

		return new Dictionary<string, object> {
			["command"] = rawCommand,
			["name"] = name,
			["arguments"] = arguments
		};
	}
}
}