using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Godotcraft.scripts.network.protocol.status.server {
public class StatusResponsePacket : Packet {
	public String content { get; private set; }
	public StatusResponse response { get; private set; }
	public StatusResponsePacket() : base(PacketType.ToClient.Status.statusResponse) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		content = dataTypes.ReadNextString(data);
		response = JsonConvert.DeserializeObject<StatusResponse>(content);
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}

	public override string ToString() {
		return $"{nameof(content)}: {content}";
	}

	public class StatusResponse {
		public Version version { get; set; }

		public Players players { get; set; }

		// public Description description { get; set; }
		public String favicon { get; set; }

		public override string ToString() {
			return $"{nameof(version)}: {version}, {nameof(players)}: {players}, {nameof(favicon)}: {favicon}";
		}
	}

	public class Version {
		public String name { get; set; }
		public int protocol { get; set; }

		public override string ToString() {
			return $"{nameof(name)}: {name}, {nameof(protocol)}: {protocol}";
		}
	}

	public class Players {
		public int max { get; set; }
		public int online { get; set; }
		public Player[] sample { get; set; }

		public override string ToString() {
			return $"{nameof(max)}: {max}, {nameof(online)}: {online}, {nameof(sample)}: {sample}";
		}
	}

	public class Player {
		public String name { get; set; }
		public String id { get; set; }

		public override string ToString() {
			return $"{nameof(name)}: {name}, {nameof(id)}: {id}";
		}
	}
}
}