using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.login.client {
public class ClientSettingsPacket : Packet {
	public String locale { get; }
	public byte viewDistance { get; }
	public int chatMode { get; }
	public bool chatColors { get; }
	public byte skinParks { get; }
	public int mainHand { get; }

	public ClientSettingsPacket(string locale, byte viewDistance, int chatMode, bool chatColors, byte skinParks, int mainHand) : base(
		PacketType.ToServer.Play.clientSettings) {
		this.locale = locale;
		this.viewDistance = viewDistance;
		this.chatMode = chatMode;
		this.chatColors = chatColors;
		this.skinParks = skinParks;
		this.mainHand = mainHand;
	}

	public ClientSettingsPacket() : base(PacketType.ToServer.Play.clientSettings) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.ConcatBytes(
			dataTypes.GetString(locale),
			dataTypes.GetByte(viewDistance),
			dataTypes.GetVarInt(chatMode),
			dataTypes.GetBool(chatColors),
			dataTypes.GetByte(skinParks),
			dataTypes.GetVarInt(mainHand)
		);
	}
}
}