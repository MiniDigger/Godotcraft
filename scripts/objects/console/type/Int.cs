namespace Godotcraft.scripts.objects.console.type {
public class Int : BaseType {
	public Int() : base("int") { }
	public override void normalized(object originalValue) {
		_normalizedValue = int.Parse(_rematch.get_string());
	}
}
}