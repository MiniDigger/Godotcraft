namespace Godotcraft.scripts.objects.console.type {
public class Any : BaseType {
	public Any() : base("Any", null) { }

	public override CHECK check(string originalValue) {
		return CHECK.OK;
	}

	public override void normalized(string originalValue) {
		_normalizedValue = originalValue;
	}
}
}