using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class PlayerAbilitiesPacket : Packet {
	
	public byte flags { get; private set; }
	public float flyingSpeed { get; private set; }
	public float walkingSpeed { get; private set; }
	
	public PlayerAbilitiesPacket() : base(PacketType.ToClient.Play.playerAbilities) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		flags = dataTypes.ReadNextByte(data);
		flyingSpeed = dataTypes.ReadNextFloat(data);
		walkingSpeed = dataTypes.ReadNextFloat(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}

	public override string ToString() {
		return $"{base.ToString()}, {nameof(flags)}: {flags}, {nameof(flyingSpeed)}: {flyingSpeed}, {nameof(walkingSpeed)}: {walkingSpeed}";
	}
}
}