namespace Godotcraft.scripts.objects.console.type {
public class String : BaseType{
	public String() : base("String") { }
	public override void normalized(object originalValue) {
		_normalizedValue = originalValue.ToString();
	}
}
}