using Godot;

namespace Godotcraft.scripts.screens {
public class MainMenu : Control {
	public override void _Ready() {
		Input.SetMouseMode(Input.MouseMode.Visible);
		
		Button playButton = GetNode("CenterContainer/VBoxContainer/Play") as Button;
		Button optionsButton = GetNode("CenterContainer/VBoxContainer/Options") as Button;
		Button quitButton = GetNode("CenterContainer/VBoxContainer/Quit") as Button;

		playButton?.Connect("pressed", this, nameof(onPlay));
		optionsButton?.Connect("pressed", this, nameof(onOptions));
		quitButton?.Connect("pressed", this, nameof(onQuit));
	}

	public void onPlay() {
		GetTree().ChangeScene("res://screens/ServerListScreen.tscn");
	}

	public void onOptions() {
		GetTree().ChangeScene("res://screens/OptionsScreen.tscn");
	}

	public void onQuit() {
		GetTree().Quit();
	}
}
}