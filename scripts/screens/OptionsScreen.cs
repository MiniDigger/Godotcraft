using Godot;

namespace Godotcraft.scripts.screens {
public class OptionsScreen : Control {
	public override void _Ready() {
		Button backButton = GetNode("CenterContainer/HBoxContainer/Back") as Button;

		backButton?.Connect("pressed", this, nameof(onBack));
	}

	public void onBack() {
		GetTree().ChangeScene("res://screens/MainMenu.tscn");
	}
}
}