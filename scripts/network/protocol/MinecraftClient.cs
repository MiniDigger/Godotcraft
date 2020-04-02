using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Godotcraft.scripts.network.protocol {
public class MinecraftClient : Node {
	private static DataTypes dataTypes = new DataTypes(578);

	private StreamPeerTCP client;

	private Queue<Packet> packetQueue = new Queue<Packet>();

	private PacketState currentState = PacketState.HANDSHAKING;

	private Dictionary<PacketType, List<object>> packetListeners = new Dictionary<PacketType, List<object>>();

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

	private void handlePacket(Packet packet) {
		if (packet == null) {
			return;
		}

		if (!packetListeners.ContainsKey(packet.type)) {
			GD.Print("no packet listeners for " + packet.type);
			return;
		}

		foreach (var action in packetListeners[packet.type]
			.Where(action => action.GetType().GetGenericArguments()[0].BaseType?.Name == nameof(Packet))) {
			action.GetType().GetMethod("Invoke")?.Invoke(action, new object[] {packet});
		}
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
		PacketType type;
		try {
			type = PacketType.of(packetId, currentState, PacketDirection.TO_CLIENT);
		}
		catch (KeyNotFoundException e) {
			GD.Print("Coulnt find a packet type for id " + packetId + " and state " + currentState);
			return null;
		}

		Packet packet = type.instance();
		packet.read(dataTypes, data);
		return packet;
	}

	public void switchState(PacketState state) {
		GD.Print("Changing state to " + state);
		currentState = state;
	}

	public void addPacketListener<T>(Action<T> action) where T : Packet {
		PacketType type = PacketType.of(typeof(T));
		GD.Print("registering packet listener for type " + type);
		List<object> listeners = packetListeners.ContainsKey(type) ? packetListeners[type] : new List<object>();
		listeners.Add(action);
		packetListeners.Add(type, listeners);
	}
}
}