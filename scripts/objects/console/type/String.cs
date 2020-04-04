namespace Godotcraft.scripts.objects.console.type {
public class String : BaseType{
	public String() : base("String", null) { }
	public override void normalized(string originalValue) {
		_normalizedValue = originalValue;
	}

	public override CHECK check(string originalValue) {
		return CHECK.OK;
	}
}
}