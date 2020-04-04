namespace Godotcraft.scripts.objects.console.type {
public class Bool : BaseType {
	public Bool() : base("Bool", "^(1|0|true|false)$") { }

	public override void normalized(string originalValue) {
		if (_rematch.GetString().Equals("1") || _rematch.GetString().Equals( "true")) {
			_normalizedValue = true;
		}
		else {
			_normalizedValue = false;
		}
	}
}
}