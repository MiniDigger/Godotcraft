using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Godotcraft.scripts.network.protocol {
public class MinecraftClient : Node {
	private static DataTypes dataTypes = new DataTypes(578);

	private StreamPeerTCP client;

	private readonly Queue<Packet> packetQueue = new Queue<Packet>();

	private PacketState currentState = PacketState.HANDSHAKING;

	private readonly Dictionary<PacketType, List<object>> packetListeners = new Dictionary<PacketType, List<object>>();

	public int compressionThreshold { get; set; }

	public override void _Ready() {
		client = new StreamPeerTCP();

		compressionThreshold = -1;

		// force init
		PacketType.init();
	}

	public Error connect(String host, int port) {
		SetProcess(true);
		return client.ConnectToHost(host, port);
	}

	public void clearPacketListeners() {
		packetListeners.Clear();
	}

	public void disconnect() {
		client.DisconnectFromHost();
	}

	public void sendPacket(Packet packet) {
		packetQueue.Enqueue(packet);
	}

	public override void _Process(float delta) {
		if (!client.IsConnectedToHost()) {
			SetProcess(false);
			return;
		}

		// read incoming packet
		while (client.GetAvailableBytes() > 0) {
			// read len
			int len = dataTypes.ReadNextVarIntFromStream(client);
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
			// GD.Print("no packet listeners for " + packet.type);
			return;
		}

		foreach (var action in packetListeners[packet.type]
			.Where(action => action.GetType().GetGenericArguments()[0].BaseType?.Name == nameof(Packet))) {
			action.GetType().GetMethod("Invoke")?.Invoke(action, new object[] {packet});
		}
	}

	private void sendPacketInternal(Packet packet) {
		byte[] packetId = dataTypes.GetVarInt(packet.type.Id);
		byte[] data = packet.write(dataTypes);


		int dataLength = dataTypes.ConcatBytes(packetId, data).Length;
		byte[] dataLen = dataTypes.GetVarInt(dataLength);

		GD.Print($"Sending packet {packet} with len {dataLength}");

		// TODO compression
		if (compressionThreshold != -1) {
			int packetLength = dataTypes.ConcatBytes(dataLen, packetId, data).Length;
			byte[] packetLen = dataTypes.GetVarInt(packetLength);

			foreach (byte b in packetLen) {
				client.PutU8(b);
			}

			if (dataLength >= compressionThreshold) {
				foreach (byte b in dataLen) {
					client.PutU8(b);
				}
				GD.Print("Need to send packet compressed!");
			}
			else {
				client.PutU8(0);
			}
		}
		else {
			foreach (byte b in dataLen) {
				client.PutU8(b);
			}
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

		// compression is compressed, lets see if the packet was actually compressed
		if (compressionThreshold != -1) {
			int dataLength = dataTypes.ReadNextVarInt(data);
			if (dataLength != 0) {
				// packet is compressed, lets decompress
				GD.Print("packet is compressed :/");
				return null;
			}
		}

		// read id
		int packetId = dataTypes.ReadNextVarInt(data);
		// GD.Print($"Got packet with id 0x{packetId:X} and state {currentState}, len is {len}");
		// handle packet
		PacketType type;
		try {
			type = PacketType.of(packetId, currentState, PacketDirection.TO_CLIENT);
		}
		catch (KeyNotFoundException e) {
			GD.Print($"Coulnt find a packet type for id 0x{packetId:X} and state {currentState}");
			return null;
		}

		Packet packet = type.instance();
		try {
			packet.read(dataTypes, data);
		}
		catch (Exception ex) {
			GD.Print($"Error while reading packet {type}: \n{ex.GetType().Name}: {ex.Message}");
			GD.Print(ex.StackTrace);
		}

		if (data?.Count != 0) {
			GD.Print($"Error while reading packet {type}: Got {data?.Count} bytes too much");
		}
		
		return packet;
	}

	public void switchState(PacketState state) {
		GD.Print("Changing state to " + state);
		currentState = state;
	}

	public void addPacketListener<T>(Action<T> action) where T : Packet {
		PacketType type = PacketType.of(typeof(T));
		// GD.Print("registering packet listener for type " + type);
		List<object> listeners = packetListeners.ContainsKey(type) ? packetListeners[type] : new List<object>();
		listeners.Add(action);
		packetListeners[type] = listeners;
	}
}
}