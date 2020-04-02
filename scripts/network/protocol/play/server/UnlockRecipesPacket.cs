using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class UnlockRecipesPacket : Packet {
	public int action { get; private set; }
	public bool craftingBookOpen { get; private set; }
	public bool craftingBookFiler { get; private set; }
	public bool smeltingBookOpen { get; private set; }
	public bool smeltingBookFilter { get; private set; }
	public String[] recipes { get; private set; }
	public String[] recipes2 { get; private set; }

	public UnlockRecipesPacket() : base(PacketType.ToClient.Play.unlockRecipes) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		action = dataTypes.ReadNextInt(data);
		craftingBookOpen = dataTypes.ReadNextBool(data);
		craftingBookFiler = dataTypes.ReadNextBool(data);
		smeltingBookOpen = dataTypes.ReadNextBool(data);
		smeltingBookFilter = dataTypes.ReadNextBool(data);
		// TODO read arrays
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}