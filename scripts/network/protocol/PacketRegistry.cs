using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.network.protocol.handshake;
using Godotcraft.scripts.network.protocol.status;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.network.protocol.status.server;

namespace Godotcraft.scripts.network.protocol {
public class PacketRegistry {
	private readonly Dictionary<(int, PacketState, PacketDirection), Type> registry =
		new Dictionary<(int, PacketState, PacketDirection), Type>();

	public PacketRegistry(int protocolVersion) {
		// HANDSHAKING
		// TO_CLIENT
		// TO_SERVER
		add(new HandshakePacket());

		// STATUS
		// TO_CLIENT
		add(new StatusResponsePacket());
		add(new PongPacket());
		// TO_SERVER
		add(new StatusRequestPacket());
		add(new PingPacket());
	}

	public void add(Packet packet) {
		registry.Add((packet.id, packet.state, packet.direction), packet.GetType());
	}

	public Packet getPacketById(int id, PacketState state, PacketDirection direction) {
		return (Packet) Activator.CreateInstance(registry[(id, state, direction)]);
	}
}
}