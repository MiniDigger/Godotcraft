using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.world;

namespace Godotcraft.scripts.network.protocol.play.server {
public class ChunkDataPacket : Packet {
	public Vector2 chunkPos { get; private set; }
	public bool fullChunk { get; private set; }
	public ChunkData chunkData { get; private set; }
	public List<Dictionary<string, object>> blockEntities { get; private set; }

	public ChunkDataPacket() : base(PacketType.ToClient.Play.chunkData) { }

	public override void read(DataTypes dataTypes, List<byte> data) {
		chunkPos = new Vector2(dataTypes.ReadNextInt(data), dataTypes.ReadNextInt(data));
		fullChunk = dataTypes.ReadNextBool(data);
		chunkData = new ChunkData();
		chunkData.read(dataTypes, data);
		int num = dataTypes.ReadNextVarInt(data);
		blockEntities = new List<Dictionary<string, object>>();
		for (var i = 0; i < num; i++) {
			blockEntities.Add(dataTypes.ReadNextNbt(data));
		}
	}

	public override byte[] write(DataTypes dataTypes) {
		throw new System.NotImplementedException();
	}
}
}