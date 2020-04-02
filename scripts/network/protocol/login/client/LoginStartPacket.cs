using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.login.client {
public class LoginStartPacket : Packet {
	public String username { get; }

	public LoginStartPacket(String username) : base(PacketType.ToServer.Login.loginStart) {
		this.username = username;
	}

	public LoginStartPacket() : base(PacketType.ToServer.Login.loginStart) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.GetString(username);
	}
}
}