using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.client {
public class TeleportConfirmPacket : Packet {
	public int teleportId { get; private set; }

	public TeleportConfirmPacket() : base(PacketType.ToServer.Play.teleportConfirm) { }

	public TeleportConfirmPacket(int packetTeleportId) : base(PacketType.ToServer.Play.teleportConfirm) {
		teleportId = packetTeleportId;
	}

	public override void read(DataTypes dataTypes, List<byte> data) {
		throw new System.NotImplementedException();
	}

	public override byte[] write(DataTypes dataTypes) {
		return dataTypes.GetVarInt(teleportId);
	}
}
}