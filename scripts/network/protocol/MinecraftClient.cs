using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Godotcraft.scripts.network.protocol {
public class MinecraftClient : Node {
	private static DataTypes dataTypes = new DataTypes(578);

	private StreamPeerTCP client;

	private Queue<Packet> packetQueue = new Queue<Packet>();

	public override void _Ready() {
		client = new StreamPeerTCP();

		// force init
		PacketType.init();
	}

	public Error connect(String host, int port) {
		return client.ConnectToHost(host, port);
	}

	public void disconnect() {
		client.DisconnectFromHost();
	}

	public void sendPacket(Packet packet) {
		packetQueue.Enqueue(packet);
	}

	public override void _Process(float delta) {
		if (!client.IsConnectedToHost()) {
			return;
		}

		// read incoming packet
		while (client.GetAvailableBytes() > 0) {
			// read len
			int len = dataTypes.ReadNextVarIntFromStream(client);
			GD.Print("Packet len " + len);
			if (len == 0) {
				GD.Print("Got zero length packet");
			}
			else {
				handlePacket(readPacketInternal(len));
			}
		}

		// send queued packets
		while (packetQueue.Count > 0) {
			sendPacketInternal(packetQueue.Dequeue());
		}
	}

	public void handlePacket(Packet packet) {
		GD.Print("Process packet " + packet);
	}

	private void sendPacketInternal(Packet packet) {
		GD.Print("Sending packet with id " + packet.type);
		byte[] packetId = dataTypes.GetVarInt(packet.type.Id);
		byte[] data = packet.write(dataTypes);
		byte[] len = dataTypes.GetVarInt(dataTypes.ConcatBytes(packetId, data).Length);

		foreach (byte b in len) {
			client.PutU8(b);
		}

		foreach (byte b in packetId) {
			client.PutU8(b);
		}

		foreach (byte b in data) {
			client.PutU8(b);
		}
	}

	private Packet readPacketInternal(int len) {
		// read data
		byte[] temp = client.GetData(len)[1] as byte[];
		List<byte> data = temp?.ToList();
		// read id
		int packetId = dataTypes.ReadNextVarInt(data);
		// handle packet
		GD.Print("Packet ID " + packetId);
		Packet packet = PacketType.of(packetId, PacketState.STATUS, PacketDirection.TO_CLIENT).instance();
		packet.read(dataTypes, data);
		GD.Print("Packet: " + packet);
		return packet;
	}
}
}