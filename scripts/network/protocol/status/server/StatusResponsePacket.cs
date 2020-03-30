using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.status.server {
public class StatusResponsePacket : Packet {
	private String content { get; set; }

	public StatusResponsePacket() : base(0, PacketState.STATUS, PacketDirection.TO_CLIENT) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		content = dataTypes.ReadNextString(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}

	public override string ToString() {
		return $"{nameof(content)}: {content}";
	}
}
}