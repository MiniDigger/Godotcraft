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

	public ChunkData() {
		// set dummy data
		for (var i = 0; i < biomes.Length; i++) {
			biomes[i] = 0; // 127 = void, 0 = ocean
		}

		for (var i = 0; i < sections.Length; i++) {
			sections[i] = new ChunkSection();
		}

//        for(ChunkSection section : sections) {
		// for (int i = 0; i < 16; i++) {
		// 	sections[0].set(index(8, i, 8), i);
		// }

		for (int i = 0; i < 16; i++) {
			for (int j = 0; j < 16; j++) {
				sections[0].set(index(j, 0, 0), 1);
			}
		}

		// for (int i = 0; i < 16; i++) {
		// 	for (int j = 0; j < 16; j++) {
		// 		sections[0].set(index(j, 3, i), 2);
		// 	}
		// }

//        }
	}

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

	public void read(DataTypes dataTypes, List<byte> data) {
		int primBitMask = dataTypes.ReadNextVarInt(data);
		var heightMapNbt = dataTypes.ReadNextNbt(data);
		if (heightMapNbt.ContainsKey("MOTION_BLOCKING")) {
			object[] map = (object[]) heightMapNbt["MOTION_BLOCKING"];
			for (var i = 0; i < map.Length; i++) {
				// need to convert the 36 longs into our 256 bytes at 9 bits per entry
				// heightMap[i] = (long) map[i];
			}
		}
		
		int biomesSize = dataTypes.ReadNextVarInt(data);
		biomes = new int[biomesSize];
		for (var i = 0; i < biomesSize; i++) {
			biomes[i] = dataTypes.ReadNextInt(data);
		}

		int dataSize = dataTypes.ReadNextVarInt(data);
		int sectionCount = numberOfSetBits(primBitMask);
		sections = new ChunkSection[sectionCount];
		for (var i = 0; i < sectionCount; i++) {
			ChunkSection section = new ChunkSection();
			section.read(dataTypes, data);
			sections[i] = section;
		}

		int blockEntityCount = dataTypes.ReadNextVarInt(data);
		for (var i = 0; i < blockEntityCount; i++) {
			blockEntities.Add(dataTypes.ReadNextNbt(data));
		}
	}
	
	int numberOfSetBits(int i)
	{
		// https://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
		i -= (i >> 1) & 0x55555555;
		i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
		return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
	}
}
}