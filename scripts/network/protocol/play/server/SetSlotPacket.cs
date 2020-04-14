using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol.play.server {
public class SetSlotPacket : Packet {

	public byte windowId { get; private set; }
	public short slot{ get; private set; }

	public SetSlotPacket() : base(PacketType.ToClient.Play.setSlot) { }
	public override void read(DataTypes dataTypes, List<byte> data) {
		windowId = dataTypes.ReadNextByte(data);
		slot = dataTypes.ReadNextShort(data);
		
		data.Clear();
		// TODO read actual data
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}