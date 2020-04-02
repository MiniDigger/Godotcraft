using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.login.server {
public class SetCompressionPacket : Packet {
	
	public int threshold { get; private set; }
	
	public SetCompressionPacket() : base(PacketType.ToClient.Login.setCompression) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		threshold = dataTypes.ReadNextVarInt(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}