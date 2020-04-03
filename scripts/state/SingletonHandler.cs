using Godot;
using Godotcraft.scripts.network.protocol;

namespace Godotcraft.scripts.state {
public class SingletonHandler : Node {

	public static SingletonHandler instance { get; private set; }
	
	public ServerManager serverManager { get; private set; }

	public MinecraftClient minecraftClient { get; private set; }
	
	public GameClient gameClient { get; private set; }
	
	public override void _Ready() {
		instance = this;
		serverManager = new ServerManager();
		gameClient = new GameClient();
		
		serverManager.load();
		
		minecraftClient = new MinecraftClient();
		AddChild(minecraftClient);
	}

	public MinecraftClient newMinecraftClient() {
		MinecraftClient mcClient = new MinecraftClient();
		AddChild(mcClient);
		return mcClient;
	}

	public void removeMinecraftClient(MinecraftClient client) {
		RemoveChild(client);
	}
}
}