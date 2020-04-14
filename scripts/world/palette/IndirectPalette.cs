using System;
using System.Collections.Generic;
using Godot;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.world.block;

namespace Godotcraft.scripts.world.palette {
public class IndirectPalette : Palette{
	Dictionary<uint, BlockState> idToState;
	Dictionary<BlockState, uint> stateToId;
	byte bitsPerBlock;

	public IndirectPalette(byte palBitsPerBlock) {
		bitsPerBlock = palBitsPerBlock;
	}

	public uint IdForState(BlockState state) {
		return stateToId[state];
	}

	public BlockState StateForId(uint id) {
		if (id >= idToState.Count) {
			GD.Print("Couldn't find state for id " + id + " (palette size is " + idToState.Count + ")");
			return BlockRegistry.AIR;
		}
		return idToState[id];
	}

	public byte GetBitsPerBlock() {
		return bitsPerBlock;
	}

	public void Read(DataTypes dataTypes, List<byte> data) {
		idToState = new Dictionary<uint, BlockState>();
		stateToId = new Dictionary<BlockState, uint>();
		// Palette Length
		int length = dataTypes.ReadNextVarInt(data);
		// Palette
		for (uint id = 0; id < length; id++) {
			uint stateId = (uint) dataTypes.ReadNextVarInt(data);
			BlockState state = BlockRegistry.GetStateFromGlobalPaletteID(stateId);
			idToState[id] = state;
			stateToId[state] = id;
		}
	}
}
}