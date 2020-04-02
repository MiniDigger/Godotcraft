using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class PlayerPositionAndLookServerPacket : Packet {
	public double x { get; private set; }
	public double y { get; private set; }
	public double z { get; private set; }
	public float yaw { get; private set; }
	public float pitch { get; private set; }
	public byte flags { get; private set; }
	public int teleportId { get; private set; }

	public PlayerPositionAndLookServerPacket() : base(PacketType.ToClient.Play.playerPositionAndLook) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		x = dataTypes.ReadNextDouble(data);
		y = dataTypes.ReadNextDouble(data);
		z = dataTypes.ReadNextDouble(data);
		yaw = dataTypes.ReadNextFloat(data);
		pitch = dataTypes.ReadNextFloat(data);
		flags = dataTypes.ReadNextByte(data);
		teleportId = dataTypes.ReadNextVarInt(data);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}