namespace Godotcraft.scripts.world.block {
public class BlockRegistry {
	
	public static double TotalNumberOfStates = 11336;
	
	public static BlockState AIR = new BlockState();
	public static BlockState DIRT = new BlockState();

	public static BlockState GetStateFromGlobalPaletteID(uint id) {
		return DIRT;
	}

	public static uint GetGlobalPaletteIDFromState(BlockState state) {
		if (state == AIR) {
			return 0;
		}
		else {
			return 1;
		}
	}
}
}