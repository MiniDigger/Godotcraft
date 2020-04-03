using Godot;
using System;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.state;

public class GameScreen : Node {
	public override void _Ready() {
		Server server = SingletonHandler.instance.stateHandler.currentServer;
		if (server == null) {
			GD.Print("No server found?!");
			return;
		}
		SingletonHandler.instance.gameClient.joinGame(server.host, server.port, "MiniDiggerTest");
		
		Node console = GetParent().GetNode("Console");
		SingletonHandler.instance.minecraftClient.addPacketListener<ChatMessageServerPacket>(packet => {
			GD.Print("Got message " + packet.message);
			console.Call("writeLine",  "[CHAT] " + packet.message);
		});
		
		// TODO add cmd
// # 1. argument is command name
// # 2. arg. is target
// # 3. arg. is target name
// 		Console.addCommand('sayHello', self, 'printHello')\
// 			.setDescription('Prints "Hello %name%!"')\
// 			.addArgument('name', TYPE_STRING)\
// 			.register()
	}
}