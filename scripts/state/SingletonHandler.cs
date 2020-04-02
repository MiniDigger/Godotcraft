using Godot;
using Godotcraft.scripts.network.protocol;

namespace Godotcraft.scripts.state {
public class SingletonHandler : Node {

	public static SingletonHandler instance { get; private set; }
	
	public ServerManager serverManager { get; private set; }

	public MinecraftClient client { get; private set; }
	
	public override void _Ready() {
		instance = this;
		serverManager = new ServerManager();
		
		client = new MinecraftClient();
		AddChild(client);
	}
}
}