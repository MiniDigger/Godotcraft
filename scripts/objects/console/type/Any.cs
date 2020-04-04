namespace Godotcraft.scripts.objects.console.type {
public class Any : BaseType {
	public Any() : base("Any") { }

	public override CHECK check(object originalValue) {
		return CHECK.OK;
	}

	public override void normalized(object originalValue) {
		_normalizedValue = originalValue;
	}
}
}