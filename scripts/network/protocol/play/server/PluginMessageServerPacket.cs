using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class PluginMessageServerPacket : Packet {
	public String channel { get; private set; }
	public List<byte> data { get; private set; }

	public PluginMessageServerPacket() : base(PacketType.ToClient.Play.pluginMessage) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		channel = dataTypes.ReadNextString(data);
		this.data = data;
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}