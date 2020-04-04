using Godot;
using System;
using Godotcraft.scripts.network.protocol.play.client;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.state;
using Godotcraft.scripts.util;

public class GameScreen : Node {
	public override void _Ready() {
		Server server = SingletonHandler.instance.stateHandler.currentServer;
		if (server == null) {
			GD.Print("No server found?!");
			return;
		}
		SingletonHandler.instance.gameClient.joinGame(server.host, server.port, "MiniDiggerTest");
		
		Node console = GetParent().GetNode("Console");
		// ConsoleUtil.addCommand(console, "say", this, nameof(onChatCommand), "Say something", "text");
		SingletonHandler.instance.minecraftClient.addPacketListener<ChatMessageServerPacket>(packet => {
			GD.Print("Got message " + packet.message);
			// ConsoleUtil.writeToConsole(console, "[CHAT] " + packet.message);
		});
		
	}

	public void onChatCommand() {
		SingletonHandler.instance.minecraftClient.sendPacket(new ChatMessageClientPacket("Hello World!"));
	}
}