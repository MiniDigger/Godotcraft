using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.status.client {
public class PingPacket : Packet {

	private long payload { get; }

	public PingPacket() : base(1, PacketState.STATUS, PacketDirection.TO_SERVER) {
		payload = 1337;
	}
	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.GetLong(payload);
	}
}
}