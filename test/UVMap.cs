using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Godotcraft.test {
public class UVMap {

	private static readonly List<UVMap> maps = new List<UVMap>();
	
	public string name { get; }
	public Vector2[] uvMap { get; }

	public UVMap(string name, Vector2[] uvMap) {
		this.name = name;
		this.uvMap = uvMap;
	}

	public void register() {
		maps.Add(this);
	}

	public static UVMap getMap(string name) {
		foreach (var map in maps) {
			if (map.name.Equals(name)) {
				return map;
			}
		}

		GD.Print("Didnt find UVMap for " + name);
		return maps[0];
	}

	public override string ToString() {
		return $"{nameof(name)}: {name}, {nameof(uvMap)}: {uvMap[0]},{uvMap[1]}";
	}
}
}