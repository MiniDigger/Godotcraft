using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol {
public abstract class Packet {
	public PacketType type { get; }

	protected Packet(PacketType type) {
		this.type = type;
	}
	
	public abstract void read(DataTypes dataTypes, List<byte> data);

	public abstract byte[] write(DataTypes dataTypes);

	public override string ToString() {
		return $"{nameof(type)}: {type}";
	}
}
}