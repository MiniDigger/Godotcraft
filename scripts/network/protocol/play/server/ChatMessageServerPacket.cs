using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class ChatMessageServerPacket : Packet{
	
	public String message { get; private set; }
	public byte position { get; private set; }
	
	public ChatMessageServerPacket() : base(PacketType.ToClient.Play.chatMessage) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		message = dataTypes.ReadNextString(data);
		position = dataTypes.ReadNextByte(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}