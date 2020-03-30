using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Godotcraft.scripts.network.protocol {
public class Util {
	private static DataTypes dataTypes = new DataTypes(578);
	private static PacketRegistry registry = new PacketRegistry(578);

	public static Error writePacket(Packet packet, StreamPeerTCP stream) {
		GD.Print("Sending packet with id " + packet.id);
		byte[] packetId = dataTypes.GetVarInt(packet.id);
		byte[] data = packet.write(dataTypes);
		byte[] len = dataTypes.GetVarInt(dataTypes.ConcatBytes(packetId, data).Length);

		foreach (byte b in len) {
			stream.PutU8(b);
		}

		foreach (byte b in packetId) {
			stream.PutU8(b);
		}

		foreach (byte b in data) {
			stream.PutU8(b);
		}

		return Error.Ok;
	}

	public static void readPacket(StreamPeerTCP stream) {
		// read len
		int len = dataTypes.ReadNextVarIntFromStream(stream);
		GD.Print("Packet len " + len);
		if (len == 0) {
			GD.Print("Got zero length packet");
			return;
		}
		// read data
		byte[] temp = stream.GetData(len)[1] as byte[];
		List<byte> data = temp?.ToList();
		// read id
		int packetId = dataTypes.ReadNextVarInt(data);
		// handle packet
		GD.Print("Packet ID " + packetId);
		Packet packet = registry.getPacketById(packetId, PacketState.STATUS, PacketDirection.TO_CLIENT);
		packet.read(dataTypes, data);
		GD.Print("Packet: " + packet);
	}
}
}