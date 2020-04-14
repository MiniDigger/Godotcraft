using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.network.protocol;

namespace Godotcraft.scripts.world {
public class ChunkSection {
	private short blockCount = 16 * 16 * 16;
	private int bitsPerBlock = 14; // direct, no palette, for now
	private int[] palette;
	private long[] data;

	public int size { get; private set; }
	private long maxEntryValue;

	public ChunkSection() {
		data = new long[(16 * 16 * 16) * bitsPerBlock / 64];

		size = data.Length * 64 / bitsPerBlock;
		maxEntryValue = (1L << bitsPerBlock) - 1;
	}

	public void set(int index, int value) {
		if (index < 0 || index > size - 1) {
			throw new IndexOutOfRangeException();
		}

		if (value < 0 || value > maxEntryValue) {
			throw new ArgumentException("Value cannot be outside of accepted range.");
		}

		int bitIndex = index * bitsPerBlock;
		int startIndex = bitIndex / 64;
		int endIndex = ((index + 1) * bitsPerBlock - 1) / 64;
		int startBitSubIndex = bitIndex % 64;
		data[startIndex] = data[startIndex] & ~(maxEntryValue << startBitSubIndex) |
		                   ((long) value & maxEntryValue) << startBitSubIndex;
		if (startIndex != endIndex) {
			int endBitSubIndex = 64 - startBitSubIndex;
			data[endIndex] = (int) ((uint) data[endIndex] >> endBitSubIndex) << endBitSubIndex | ((long) value & maxEntryValue) >> endBitSubIndex;
		}
	}

	public int get(int index) {
		if (index < 0 || index > size - 1) {
			// throw new IndexOutOfRangeException();
			return 0;
		}

		int bitIndex = index * bitsPerBlock;
		int startIndex = bitIndex / 64;
		int endIndex = ((index + 1) * bitsPerBlock - 1) / 64;
		int startBitSubIndex = bitIndex % 64;
		if (startIndex == endIndex) {
			return (int) ((uint) data[startIndex] >> startBitSubIndex & maxEntryValue);
		}
		else {
			int endBitSubIndex = 64 - startBitSubIndex;
			return (int) (((uint) data[startIndex] >> startBitSubIndex | data[endIndex] << endBitSubIndex) & maxEntryValue);
		}
	}

	public bool isEmpty() {
		return blockCount == 0;
	}

	public void read(DataTypes dataTypes, List<byte> dataBytes) {
		blockCount = dataTypes.ReadNextShort(dataBytes);
		bitsPerBlock = dataTypes.ReadNextByte(dataBytes);
		if (bitsPerBlock <= 0) bitsPerBlock = 4;
		palette = new int[dataTypes.ReadNextVarInt(dataBytes)];
		for (var i = 0; i < palette.Length; i++) {
			palette[i] = dataTypes.ReadNextVarInt(dataBytes);
		}
		data = new long[dataTypes.ReadNextVarInt(dataBytes)];
		if (data.Length == 0 || bitsPerBlock == 0) {
			size = 0;
		}
		else {
			size = data.Length * 64 / bitsPerBlock;
		}
		for (var i = 0; i < data.Length; i++) {
			data[i] = dataTypes.ReadNextLong(dataBytes);
		}
	}
}
}