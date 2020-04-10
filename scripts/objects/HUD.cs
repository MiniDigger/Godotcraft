using Godot;

public class HUD : CanvasLayer {

	private Label fpsCounter;
	
    public override void _Ready() {
	    fpsCounter = GetNode<Label>("FPS");
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta) {
	  fpsCounter.Text = "FPS: " + Engine.GetFramesPerSecond();
  }
}
