using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godotcraft.scripts.network.protocol.login.client;
using Godotcraft.scripts.network.protocol.login.server;
using Godotcraft.scripts.network.protocol.play.client;
using Godotcraft.scripts.network.protocol.play.server;
using Godotcraft.scripts.network.protocol.status.client;
using Godotcraft.scripts.network.protocol.status.server;
using Godotcraft.scripts.state;

namespace Godotcraft.scripts.network.protocol {
public struct PacketType {
	public struct ToClient {
		private static readonly PacketDirection dir = PacketDirection.TO_CLIENT;


		public struct Status {
			private static readonly PacketState state = PacketState.STATUS;
			public static readonly PacketType statusResponse = new PacketType(0x00, state, dir, typeof(StatusResponsePacket));
			public static readonly PacketType pong = new PacketType(1, state, dir, typeof(PongPacket));

			public static void init() {
				add(statusResponse);
				add(pong);
			}
		}

		public struct Login {
			private static readonly PacketState state = PacketState.LOGIN;
			public static readonly PacketType loginSuccess = new PacketType(0x02, state, dir, typeof(LoginSuccessPacket));

			public static void init() {
				add(loginSuccess);
			}
		}

		public struct Play {
			private static readonly PacketState state = PacketState.PLAY;
			public static readonly PacketType chatMessage = new PacketType(0x0F, state, dir, typeof(ChatMessageServerPacket));
			public static readonly PacketType playerAbilities = new PacketType(0x19, state, dir, typeof(PlayerAbilitiesPacket));
			public static readonly PacketType keepAlive = new PacketType(0x21, state, dir, typeof(KeepAliveServerPacket));
			public static readonly PacketType chunkData = new PacketType(0x22, state, dir, typeof(ChunkDataPacket));
			public static readonly PacketType joinGame = new PacketType(0x26, state, dir, typeof(JoinGamePacket));

			public static void init() {
				add(chatMessage);
				add(playerAbilities);
				add(keepAlive);
				add(chunkData);
				add(joinGame);
			}
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
			public static readonly PacketType handshake = new PacketType(0x00, state, dir, typeof(Handshake));

			public static void init() {
				add(handshake);
			}
		}

		public struct Status {
			private static readonly PacketState state = PacketState.STATUS;
			public static readonly PacketType statusRequest = new PacketType(0x00, state, dir, typeof(StatusRequestPacket));
			public static readonly PacketType ping = new PacketType(0x01, state, dir, typeof(PingPacket));

			public static void init() {
				add(statusRequest);
				add(ping);
			}
		}

		public struct Login {
			private static readonly PacketState state = PacketState.LOGIN;

			public static readonly PacketType loginStart = new PacketType(0x00, state, dir, typeof(LoginStartPacket));

			public static void init() {
				add(loginStart);
			}
		}

		public struct Play {
			private static readonly PacketState state = PacketState.PLAY;

			public static readonly PacketType chatMessage = new PacketType(0x03, state, dir, typeof(ChatMessageClientPacket));
			public static readonly PacketType clientSettings = new PacketType(0x05, state, dir, typeof(ClientSettingsPacket));
			public static readonly PacketType keepAlive = new PacketType(0x0F, state, dir, typeof(KeepAliveClientPacket));

			public static void init() {
				add(chatMessage);
				add(clientSettings);
				add(keepAlive);
			}
		}

		public static void init() {
			Handshake.init();
			Status.init();
			Login.init();
			Play.init();
		}
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
		return $"{nameof(Id)}: 0x{Id:X}, {nameof(State)}: {State}, {nameof(Direction)}: {Direction}, {nameof(Packet)}: {Packet}";
	}

	public static PacketType of(int id, PacketState state, PacketDirection direction) {
		return types[(id, state, direction)];
	}

	public static PacketType of(Type type) {
		foreach (var packetType in types.Values.Where(packetType => packetType.Packet == type)) {
			return packetType;
		}

		throw new Exception("No packet type for type " + nameof(type) + " found");
	}

	private static void add(PacketType type) {
		GD.Print("Register packet " + type);
		types.Add((type.Id, type.State, type.Direction), type);
	}
}
}