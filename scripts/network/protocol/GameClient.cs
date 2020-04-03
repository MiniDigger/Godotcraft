using System;
using Godot;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.login.client;
using Godotcraft.scripts.network.protocol.login.server;
using Godotcraft.scripts.network.protocol.play.client;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.network.protocol.status.server;
using Godotcraft.scripts.state;

namespace Godotcraft.scripts.network.protocol {
public class GameClient {
	public void joinGame(String host, ushort port, String playerName) {
		MinecraftClient minecraftClient = SingletonHandler.instance.minecraftClient;
		minecraftClient.clearPacketListeners();
		Error status = minecraftClient.connect(host, port);
		GD.Print("Connected with status: " + status);
		minecraftClient.sendPacket(new HandshakePacket(578, host, port, PacketState.LOGIN));
		minecraftClient.switchState(PacketState.LOGIN);
		minecraftClient.sendPacket(new LoginStartPacket(playerName));
		minecraftClient.addPacketListener<LoginSuccessPacket>(packet => {
			minecraftClient.switchState(PacketState.PLAY);
			// brand?
			minecraftClient.sendPacket(new ClientSettingsPacket("en_US", 8, 0, true, 0b11111111, 1));
			minecraftClient.sendPacket(new ChatMessageClientPacket("Hello world!"));
		});

		minecraftClient.addPacketListener<LoginDisconnectPacket>(packet => {
			GD.Print("Kicked: " + packet.reason);
			minecraftClient.disconnect();
		});
		minecraftClient.addPacketListener<SetCompressionPacket>(packet => {
			GD.Print("Got compression request " + packet.threshold);
			minecraftClient.compressionThreshold = packet.threshold;
		});
		minecraftClient.addPacketListener<KeepAliveServerPacket>(packet => { minecraftClient.sendPacket(new KeepAliveClientPacket(packet.id)); });
		minecraftClient.addPacketListener<ChunkDataPacket>(packet => {
			// ignore
		});
		minecraftClient.addPacketListener<PlayerPositionAndLookServerPacket>(packet => {
			minecraftClient.sendPacket(new TeleportConfirmPacket(packet.teleportId));
		});
		minecraftClient.addPacketListener<ChatMessageServerPacket>(packet => { GD.Print("Got message " + packet.message); });
		minecraftClient.addPacketListener<PluginMessageServerPacket>(packet => { GD.Print("Got plugin message in channel " + packet.channel); });
	}

	public void serverListPing(String host, ushort port, Action<StatusResponsePacket.StatusResponse> callback) {
		MinecraftClient minecraftClient = SingletonHandler.instance.newMinecraftClient();
		minecraftClient.connect(host, port);
		minecraftClient.sendPacket(new HandshakePacket(578, host, port, PacketState.STATUS));
		minecraftClient.switchState(PacketState.STATUS);
		minecraftClient.sendPacket(new StatusRequestPacket());
		minecraftClient.sendPacket(new PingPacket());
		
		minecraftClient.addPacketListener<StatusResponsePacket>(packet => {
			minecraftClient.disconnect();
			callback.Invoke(packet.response);
			SingletonHandler.instance.removeMinecraftClient(minecraftClient);
		});
	}
}
}