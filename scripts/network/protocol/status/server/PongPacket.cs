using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.status.server {
public class PongPacket : Packet{
	
	private long payload { get; set; }
	
	public PongPacket() : base(PacketType.ToClient.Status.pong) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		payload = dataTypes.ReadNextLong(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}