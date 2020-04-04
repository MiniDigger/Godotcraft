namespace Godotcraft.scripts.objects.console.type {
public class Float : BaseType{
	public Float() : base("Float") { }
	public override void normalized(object originalValue) {
		_normalizedValue = float.Parse(_rematch.get_string().replace(",", "."));
	}
}
}