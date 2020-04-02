using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.handshake {
public class HandshakePacket : Packet {
	public int protocolVersion { get; private set; }
	public String serverAddress { get; private set; }
	public ushort serverPort { get; private set; }
	public PacketState nextState { get; private set; }

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