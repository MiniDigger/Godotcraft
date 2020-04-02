using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.client {
public class PluginMessageClientPacket : Packet {
	public String channel { get; private set; }
	public List<byte> data { get; private set; }

	public PluginMessageClientPacket() : base(PacketType.ToServer.Play.pluginMessage) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.ConcatBytes(
			dataTypes.GetString(channel),
			dataTypes.GetArray(data.ToArray())
		);
	}
}
}