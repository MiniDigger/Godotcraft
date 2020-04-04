namespace Godotcraft.scripts.objects.console.type {
public class Bool : BaseType {
	public Bool() : base("Bool") { }

	public override void normalized(object originalValue) {
		if (_rematch.get_string() == "1" || _rematch.get_string() == "true") {
			_normalizedValue = true;
		}
		else {
			_normalizedValue = false;
		}
	}
}
}