using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class UpdateLightPacket : Packet{
	public UpdateLightPacket() : base(PacketType.ToClient.Play.updateLight) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		// TODO implement update light
		data.Clear();
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}