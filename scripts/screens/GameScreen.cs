using Godot;
using Godotcraft.scripts.network.protocol.play.client;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.objects;
using Godotcraft.scripts.objects.console.type;
using Godotcraft.scripts.state;

public class GameScreen : Node {
	public override void _Ready() {
		Server server = SingletonHandler.instance.stateHandler.currentServer;
		if (server == null) {
			GD.Print("No server found?!");
			return;
		}
		SingletonHandler.instance.gameClient.joinGame(server.host, server.port, "MiniDiggerTest");
		
		Console.instance.addCommand("say", text => onChatCommand(text.Count > 0 ? (string)text[0] : null))
			.setDescription("Prints a string.")
			.addArgument("text", typeof(String))
			.register();
		
		SingletonHandler.instance.minecraftClient.addPacketListener<ChatMessageServerPacket>(packet => {
			GD.Print("Got message " + packet.message);
			Console.instance.writeLine("[CHAT] " + packet.message);
		});
		
	}

	public void onChatCommand(string text) {
		if (text != null) {
			SingletonHandler.instance.minecraftClient.sendPacket(new ChatMessageClientPacket(text));
		}
	}
}