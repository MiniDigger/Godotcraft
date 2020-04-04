namespace Godotcraft.scripts.objects.console.type {
public class Float : BaseType{
	public Float() : base("Float", "^[+-]?([0-9]*[\\.]?[0-9]+|[0-9]+[\\.]?[0-9]*)([eE][+-]?[0-9]+)?$") { }
	public override void normalized(string originalValue) {
		_normalizedValue = float.Parse(_rematch.GetString().Replace(",", "."));
	}
}
}