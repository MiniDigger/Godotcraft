using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class ChunkDataPacket : Packet{
	public ChunkDataPacket() : base(PacketType.ToClient.Play.chunkData) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		// TODO implement chunk shit
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}