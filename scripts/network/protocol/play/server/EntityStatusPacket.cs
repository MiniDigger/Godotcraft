using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class EntityStatusPacket : Packet{
	
	public int entityId { get; private set; }
	public byte entityStatus { get; private set; }
	
	public EntityStatusPacket() : base(PacketType.ToClient.Play.entityStatus) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		entityId = dataTypes.ReadNextInt(data);
		entityStatus = dataTypes.ReadNextByte(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}