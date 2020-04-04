namespace Godotcraft.scripts.objects.console.type {
public class Int : BaseType {
	public Int() : base("int", "^[+-]?\\d+$") { }
	public override void normalized(string originalValue) {
		_normalizedValue = int.Parse(_rematch.GetString());
	}
}
}