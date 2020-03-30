using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.status.client {
public class StatusRequestPacket : Packet {
	public StatusRequestPacket() : base(0, PacketState.STATUS, PacketDirection.TO_SERVER) { }

	public override void read(DataTypes dataTypes, List<byte> data) { }

	public override byte[] write(DataTypes dataTypes) {
		return new byte[0];
	}
}
}