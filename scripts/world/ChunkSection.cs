using System;
using Godotcraft.scripts.network.protocol;

namespace Godotcraft.scripts.world {
public class ChunkSection {
	private short blockCount = 16 * 16 * 16;
	private int bitsPerBlock = 14; // direct, no palette, for now
	private int[] palette;
	private long[] data;

	private int size;
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
			throw new IndexOutOfRangeException();
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
		return false;
	}
}
}