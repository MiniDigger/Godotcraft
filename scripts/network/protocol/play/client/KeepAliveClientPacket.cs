using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.client {
public class KeepAliveClientPacket : Packet{
	
	public long id { get; private set; }
	
	public KeepAliveClientPacket() : base(PacketType.ToServer.Play.keepAlive) { }

	public KeepAliveClientPacket(long id) : base(PacketType.ToServer.Play.keepAlive) {
		this.id = id;
	}
	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.GetLong(id);
	}
}
}