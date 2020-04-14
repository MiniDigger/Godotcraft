using System.Collections.Generic;
using Godotcraft.scripts.network.protocol;
using Godotcraft.scripts.world.block;

namespace Godotcraft.scripts.world.palette {
public interface Palette {
	uint IdForState(BlockState state);
	BlockState StateForId(uint id);
	byte GetBitsPerBlock();
	void Read(DataTypes dataTypes, List<byte> data);
}
}