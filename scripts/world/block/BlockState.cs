namespace Godotcraft.scripts.world.block {
public class BlockState {

	public string name { get; }
	public uint id { get; }
	
	public bool transparent { get; }

	public BlockState(string name, uint id) {
		this.name = name;
		this.id = id;

		if (name.Contains("air") || name.Contains("glass")) {
			transparent = true;
		}
		else {
			transparent = false;
		}
	}

	protected bool Equals(BlockState other) {
		return name == other.name && id == other.id;
	}

	public override bool Equals(object obj) {
		if (ReferenceEquals(null, obj)) {
			return false;
		}

		if (ReferenceEquals(this, obj)) {
			return true;
		}

		if (obj.GetType() != this.GetType()) {
			return false;
		}

		return Equals((BlockState) obj);
	}

	public override int GetHashCode() {
		unchecked {
			return ((name != null ? name.GetHashCode() : 0) * 397) ^ (int) id;
		}
	}
}
}