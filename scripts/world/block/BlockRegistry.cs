using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = Godot.File;

namespace Godotcraft.scripts.world.block {
public class BlockRegistry {
	
	public static double TotalNumberOfStates = 11336;
	
	public static BlockState AIR;
	public static BlockState DIRT;

	private static Dictionary<uint, BlockState> idToState = new Dictionary<uint, BlockState>();
	private static Dictionary<BlockState, uint> stateToId = new Dictionary<BlockState, uint>();
	private static Dictionary<string, BlockState> nameToState = new Dictionary<string, BlockState>();

	static BlockRegistry() {
		loadRegistry();

		AIR = nameToState["air"];
		DIRT = nameToState["dirt"];
	}
	
	private static void loadRegistry() {
		File registryFile = new File();
		registryFile.Open("user://mcdata/reports/blocks.json", File.ModeFlags.Read);
		string content = registryFile.GetAsText();
		JObject o = JObject.Parse(content);
		foreach (var entry in o) {
			string name = entry.Key.Split(':')[1];
			JArray states = (entry.Value["states"]) as JArray;
			foreach (var state in states) {
				uint id = state["id"].Value<uint>();
				add(new BlockState(name, id));
			}
		}
		GD.Print($"Loaded {idToState.Count} states");
	}

	private static void add(BlockState state) {
		idToState[state.id] = state;
		stateToId[state] = state.id;
		nameToState[state.name] = state;
	}
	
	public static BlockState GetStateFromGlobalPaletteID(uint id) {
		return idToState[id];
	}

	public static uint GetGlobalPaletteIDFromState(BlockState state) {
		return stateToId[state];
	}
}
}