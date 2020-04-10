using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.objects.console.command;
using Godotcraft.scripts.objects.console.misc;

namespace Godotcraft.scripts.objects {
public class Console : CanvasLayer {
	public static Console instance;
	public static Logger Log = new Logger();
	public static History History = new History(10);

	public CommandGroup _rootGroup { get; }

	// Used to clear text from bb tags
	private RegEx _eraseTrash;
	public bool isConsoleShown { get; private set; }
	public bool submitAutocomplete { get; private set; }

	// console nodes
	private Control _consoleBox;
	private RichTextLabel Text;
	private LineEdit Line;
	private AnimationPlayer _animationPlayer;

	public Console() {
		_rootGroup = new CommandGroup("root");
		// Used to clear text from bb tags
		_eraseTrash = new RegEx();
		_eraseTrash.Compile("\\[[\\/]?[a-z0-9\\=\\#\\ \\_\\-\\,\\.\\;]+\\]");
	}

	public override void _Ready() {
		isConsoleShown = true;
		submitAutocomplete = true;

		instance = this;

		// load console nodes
		_consoleBox = GetNode("ConsoleBox") as Control;
		Text = GetNode("ConsoleBox/Container/ConsoleText") as RichTextLabel;
		Line = GetNode("ConsoleBox/Container/ConsoleLine") as LineEdit;
		_animationPlayer = GetNode("ConsoleBox/AnimationPlayer") as AnimationPlayer;

		// Allow selecting console text
		Text.SelectionEnabled = true;
		// Follow console output (for scrolling)
		Text.ScrollFollowing = true;
		// React to clicks on console urls
		Text.Connect("meta_clicked", Line, nameof(LineEdit.SetText));

		// Hide console by default
		_consoleBox.Hide();
		_animationPlayer.Connect("animation_finished", this, nameof(_toggleAnimationFinished));
		toggleConsole();

		// Console keyboard control
		SetProcessInput(true);

		// Show some info
		var v = Engine.GetVersionInfo();
		writeLine(
			ProjectSettings.GetSetting("application/config/name") +
			" (Godot " + v["major"] + '.' + v["minor"] + '.' + v["patch"] + ' ' + v["status"] + ")\n" +
			"Type [color=#ffff66][url=help]help[/url][/color] to get more information about usage");

		// Init base commands
		BaseCommands.init();
	}

	public override void _Input(InputEvent @event) {
		if ((!isConsoleShown && Input.IsActionJustPressed(Actions.console_open)) ||
		    (isConsoleShown && Input.IsActionJustPressed(Actions.console_close))) {
			toggleConsole();
		}
	}

	public Command getCommand(String name) {
		return _rootGroup.getCommand(name, new List<object>());
	}

	public CommandBuilder addCommand(String name, Action<List<object>> target) {
		return new CommandBuilder(_rootGroup, name, target);
	}

	public bool removeCommand(String name) {
		return _rootGroup.unregisterCommand(name);
	}

	public void write(String message) {
		if (Text != null) {
			Text.BbcodeText += message;
		}
	}

	public void writeLine(String message = null) {
		if (Text != null) {
			Text.BbcodeText += message + "\n";
		}
	}

	public void clear() {
		if (Text != null) {
			Text.BbcodeText = "";
		}
	}

	public void toggleConsole() {
		if (!isConsoleShown) {
			_consoleBox.Show();
			Line.Clear();
			Line.GrabFocus();
			_animationPlayer.PlayBackwards("fade");
			Input.SetMouseMode(Input.MouseMode.Visible);
		}
		else {
			_animationPlayer.Play("fade");
			Input.SetMouseMode(Input.MouseMode.Captured);
		}

		isConsoleShown = !isConsoleShown;
	}

	public void _toggleAnimationFinished(string animation) {
		if (!isConsoleShown) {
			_consoleBox.Hide();
		}
	}
}
}