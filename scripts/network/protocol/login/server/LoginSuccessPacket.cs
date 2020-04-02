using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.login.server {
public class LoginSuccessPacket : Packet {

	public String uuid;
	public String username;
	
	public LoginSuccessPacket() : base(PacketType.ToClient.Login.loginSuccess) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		uuid = dataTypes.ReadNextString(data);
		username = dataTypes.ReadNextString(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}