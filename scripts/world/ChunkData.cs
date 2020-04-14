using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.network.protocol;

namespace Godotcraft.scripts.world {
public class ChunkData {
	private ChunkSection[] sections = new ChunkSection[256 / 16];

	private byte[] heightMap = new byte[256 * 16];

	private int[] biomes = new int[256];

	private List<Dictionary<string, object>> blockEntities = new List<Dictionary<string, object>>();

	public static int index(int x, int y, int z) {
		return y << 8 | z << 4 | x;
	}

	public void setBiome(int x, int z, int id) {
		biomes[z * 16 | x] = id;
	}

	public int getBiome(int x, int z) {
		return biomes[z * 16 | x];
	}

	public ChunkSection getSection(int id) {
		return sections[id];
	}

	public int getSectionCount() {
		return sections.Length;
	}

	public void read(DataTypes dataTypes, List<byte> data, bool fullChunk) {
		int primBitMask = dataTypes.ReadNextVarInt(data);
		var heightMapNbt = dataTypes.ReadNextNbt(data);
		if (heightMapNbt.ContainsKey("MOTION_BLOCKING")) {
			object[] map = (object[]) heightMapNbt["MOTION_BLOCKING"];
			for (var i = 0; i < map.Length; i++) {
				// need to convert the 36 longs into our 256 bytes at 9 bits per entry
				// heightMap[i] = (long) map[i];
			}
		}

		if (fullChunk) {
			// int biomesSize = dataTypes.ReadNextVarInt(data);
			biomes = new int[1024];
			for (var i = 0; i < biomes.Length; i++) {
				biomes[i] = dataTypes.ReadNextInt(data);
			}
		}

		int dataSize = dataTypes.ReadNextVarInt(data);
		// todo if chunks exist, get it here
		sections = new ChunkSection[16];
		for (int sectionY = 0; sectionY < 16; sectionY++) {
			if ((primBitMask & (1 << sectionY)) == 0) {
				// section is empty
				continue;
			}

			ChunkSection section = new ChunkSection();
			section.read(dataTypes, data);
			sections[sectionY] = section;
		}

		int blockEntityCount = dataTypes.ReadNextVarInt(data);
		for (var i = 0; i < blockEntityCount; i++) {
			blockEntities.Add(dataTypes.ReadNextNbt(data));
		}
	}
}
}