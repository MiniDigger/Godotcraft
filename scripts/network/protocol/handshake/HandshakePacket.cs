using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.handshake {
public class HandshakePacket : Packet {
	private int protocolVersion { get; }
	private String serverAddress { get; }
	private ushort serverPort { get; }
	private PacketState nextState { get; }

	public HandshakePacket(int protocolVersion, string serverAddress, ushort serverPort, PacketState nextState) : base(PacketType.ToServer.Handshake
		.handshake) {
		this.protocolVersion = protocolVersion;
		this.serverAddress = serverAddress;
		this.serverPort = serverPort;
		this.nextState = nextState;
	}

	public HandshakePacket() : base(PacketType.ToServer.Handshake.handshake) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.ConcatBytes(
			dataTypes.GetVarInt(protocolVersion),
			dataTypes.GetString(serverAddress),
			dataTypes.GetUShort(serverPort),
			dataTypes.GetVarInt((int) nextState));
	}
}
}