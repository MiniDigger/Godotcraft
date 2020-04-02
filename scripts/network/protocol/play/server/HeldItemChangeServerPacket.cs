using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class HeldItemChangeServerPacket : Packet{
	
	public byte slot { get; private set; }
	
	public HeldItemChangeServerPacket() : base(PacketType.ToClient.Play.heldItemChange) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		slot = dataTypes.ReadNextByte(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}