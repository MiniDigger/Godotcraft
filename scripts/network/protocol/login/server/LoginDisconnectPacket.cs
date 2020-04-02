using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.login.server {
public class LoginDisconnectPacket : Packet{
	
	public String reason { get; private set; }
	
	public LoginDisconnectPacket() : base(PacketType.ToClient.Login.disconnect) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		reason = dataTypes.ReadNextString(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}