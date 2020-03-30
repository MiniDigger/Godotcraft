using System.Collections.Generic;

namespace Godotcraft.scripts.network.protocol {
public abstract class Packet {
	public int id { get; }
	public PacketState state { get; }
	public PacketDirection direction { get;  }

	protected Packet(int id, PacketState state, PacketDirection direction) {
		this.id = id;
		this.state = state;
		this.direction = direction;
	}
	
	public abstract void read(DataTypes dataTypes, List<byte> data);

	public abstract byte[] write(DataTypes dataTypes);

	public override string ToString() {
		return $"{nameof(id)}: {id}, {nameof(state)}: {state}, {nameof(direction)}: {direction}";
	}
}
}