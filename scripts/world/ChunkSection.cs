using System.Collections.Generic;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.world.block;
using Godotcraft.scripts.world.palette;

namespace Godotcraft.scripts.world {
public class ChunkSection {
	private short blockCount;
	private byte bitsPerBlock;
	private long[] data = new long[0];
	private uint individualValueMask;
	private Palette palette;

	public void set(int x, int y, int z, BlockState state) {
		set((y * 16 + z) * 16 + x, state);
	}

	public void set(int index, BlockState state) {
		int startLong = (index * bitsPerBlock) / 64;
		int startOffset = (index * bitsPerBlock) % 64;
		int endLong = ((index + 1) * bitsPerBlock - 1) / 64;

		long value = palette.IdForState(state);
		value &= individualValueMask;

		data[startLong] |= (value << startOffset);

		if (startLong != endLong) {
			data[endLong] = (value >> (64 - startOffset));
		}
	}

	public BlockState get(int x, int y, int z) {
		return get((y * 16 + z) * 16 + x);
	}

	public BlockState get(int blockNumber) {
		int startLong = blockNumber * bitsPerBlock / 64;
		int startOffset = blockNumber * bitsPerBlock % 64;
		int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;

		uint blockId;
		if (startLong == endLong) {
			blockId = (uint) (data[startLong] >> startOffset);
		}
		else {
			int endOffset = 64 - startOffset;
			blockId = (uint) (data[startLong] >> startOffset | data[endLong] << endOffset);
		}

		blockId &= individualValueMask;

		// data should always be valid for the palette
		// If you're reading a power of 2 minus one (15, 31, 63, 127, etc...) that's out of bounds,
		// you're probably reading light data instead

		return palette.StateForId(blockId);
	}

	public bool isEmpty() {
		return blockCount == 0;
	}

	public void read(DataTypes dataTypes, List<byte> dataBytes) {
		blockCount = dataTypes.ReadNextShort(dataBytes);
		bitsPerBlock = dataTypes.ReadNextByte(dataBytes);

		palette = ChoosePalette(bitsPerBlock);
		palette.Read(dataTypes, dataBytes);

		individualValueMask = (uint) ((1 << bitsPerBlock) - 1);

		int dataSize = dataTypes.ReadNextVarInt(dataBytes);
		data = new long[dataSize];
		for (var i = 0; i < data.Length; i++) {
			data[i] = dataTypes.ReadNextLong(dataBytes);
		}
	}

	private Palette ChoosePalette(byte bitsPerBlock) {
		if (bitsPerBlock <= 4) {
			return new IndirectPalette(4);
		}
		else if (bitsPerBlock <= 8) {
			return new IndirectPalette(bitsPerBlock);
		}
		else {
			return new DirectPalette();
		}
	}
}
}