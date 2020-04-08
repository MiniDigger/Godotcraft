using System;

namespace Godotcraft.scripts.world {
public class ChunkData {
	private ChunkSection[] sections = new ChunkSection[256 / 16];

	private byte[] heightMap = new byte[256 * 16];

	private int[] biomes = new int[256];

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
}
}