using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class ServerDifficultyPacket : Packet {
	
	public byte difficulty { get; private set; }
	public bool locked { get; private set; }
	
	public ServerDifficultyPacket() : base(PacketType.ToClient.Play.serverDifficulty) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		difficulty = dataTypes.ReadNextByte(data);
		locked = dataTypes.ReadNextBool(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}