using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.client {
public class ChatMessageClientPacket : Packet{
	
	public String message { get; private set; }
	
	public ChatMessageClientPacket(String message) : base(PacketType.ToServer.Play.chatMessage) {
		this.message = message;
	}
	
	public ChatMessageClientPacket() : base(PacketType.ToServer.Play.chatMessage) {
	}
	
	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.GetString(message);
	}
}
}