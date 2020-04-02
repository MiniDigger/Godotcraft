using System.Collections.Generic;
using Godot;

namespace Godotcraft.scripts.network.protocol.play.server {
public class KeepAliveServerPacket : Packet{
	
	public long id { get; private set; }
	
	public KeepAliveServerPacket() : base(PacketType.ToClient.Play.keepAlive) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		id = dataTypes.ReadNextLong(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}