using System.Collections.Generic;
using Godot;

namespace Godotcraft.scripts.renderer {
public class UVMap {

	private static readonly Dictionary<string, UVMap> maps = new Dictionary<string, UVMap>();
	
	public string name { get; }
	public Vector2[] uvMap { get; }

	public UVMap(string name, Vector2[] uvMap) {
		this.name = name;
		this.uvMap = uvMap;
	}

	public void register() {
		maps.Add(name, this);
	}

	public static UVMap getMap(string name) {
		UVMap map;
		maps.TryGetValue(name, out map);
		if (map != null) {
			return map;
		}

		// GD.Print("Didnt find UVMap for " + name);
		return maps["debug"];
	}

	public override string ToString() {
		return $"{nameof(name)}: {name}, {nameof(uvMap)}: {uvMap[0]},{uvMap[1]}";
	}
}
}