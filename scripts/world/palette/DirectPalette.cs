using System;
using System.Collections.Generic;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.world.block;

namespace Godotcraft.scripts.world.palette {
public class DirectPalette : Palette{
	public uint IdForState(BlockState state) {
		return BlockRegistry.GetGlobalPaletteIDFromState(state);
	}

	public BlockState StateForId(uint id) {
		return BlockRegistry.GetStateFromGlobalPaletteID(id);
	}

	public byte GetBitsPerBlock() {
		return (byte) Math.Ceiling(Math.Log(BlockRegistry.TotalNumberOfStates, 2)); // currently 14
	}

	public void Read(DataTypes dataTypes, List<byte> data) {
		// nothing to read
	}
}
}