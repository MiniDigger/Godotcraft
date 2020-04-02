using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.network.protocol.status.server;
using Godotcraft.scripts.state;

namespace Godotcraft.scripts.network.protocol {
public struct PacketType {
	public struct ToClient {
		private static readonly PacketDirection dir = PacketDirection.TO_CLIENT;


		public struct Status {
			private static readonly PacketState state = PacketState.STATUS;
			public static readonly PacketType statusResponse = new PacketType(0, state, dir, typeof(StatusResponsePacket));
			public static readonly PacketType pong = new PacketType(1, state, dir, typeof(PongPacket));

			public static void init() {
				add(statusResponse);
				add(pong);
			}
		}

		public struct Login {
			private static readonly PacketState state = PacketState.LOGIN;
			public static void init() { }
		}

		public struct Play {
			private static readonly PacketState state = PacketState.PLAY;
			public static void init() { }
		}

		public static void init() {
			Status.init();
			Login.init();
			Play.init();
		}
	}

	public struct ToServer {
		private static readonly PacketDirection dir = PacketDirection.TO_SERVER;

		public struct Handshake {
			private static readonly PacketState state = PacketState.HANDSHAKING;
			public static readonly PacketType handshake = new PacketType(0, state, dir, typeof(Handshake));

			public static void init() {
				add(handshake);
			}
		}

		public struct Status {
			private static readonly PacketState state = PacketState.STATUS;
			public static readonly PacketType statusRequest = new PacketType(0, state, dir, typeof(StatusRequestPacket));
			public static readonly PacketType ping = new PacketType(1, state, dir, typeof(PingPacket));

			public static void init() {
				add(statusRequest);
				add(ping);
			}
		}

		public struct Login {
			private static readonly PacketState state = PacketState.LOGIN;

			public static void init() { }
		}

		public struct Play {
			private static readonly PacketState state = PacketState.PLAY;
			public static void init() { }
		}

		public static void init() { 
			Handshake.init();
			Status.init();
			Login.init();
			Play.init();}
	}

	public static void init() {
		ToServer.init();
		ToClient.init();
	}

	private static Dictionary<(int, PacketState, PacketDirection), PacketType> types =
		new Dictionary<(int, PacketState, PacketDirection), PacketType>();

	public int Id { get; }
	public PacketState State { get; }
	public PacketDirection Direction { get; }
	public Type Packet { get; }

	private PacketType(int id, PacketState packetState, PacketDirection direction, Type type) {
		Id = id;
		State = packetState;
		Direction = direction;
		Packet = type;
	}

	public Packet instance() {
		return (Packet) Activator.CreateInstance(Packet);
	}

	public override string ToString() {
		return $"{nameof(Id)}: {Id}, {nameof(State)}: {State}, {nameof(Direction)}: {Direction}, {nameof(Packet)}: {Packet}";
	}

	public static PacketType of(int id, PacketState state, PacketDirection direction) {
		return types[(id, state, direction)];
	}

	private static void add(PacketType type) {
		GD.Print("Add " + type);
		types.Add((type.Id, type.State, type.Direction), type);
	}
}
}