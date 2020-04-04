using System;
using System.Collections.Generic;
using Godotcraft.scripts.objects.console.type;
using String = System.String;

namespace Godotcraft.scripts.objects.console.argument {
public class ArgumentFactory {

	public static Argument create(String name, Type type, String description = null) {
		// Define argument type
		if (!(typeof(type) == TYPE_OBJECT and type is BaseType)) {
			type = TypeFactory.create(typeof(type) == TYPE_INT ? type : 0);
		}

		if (type is BaseType) {
			Console.Log.error("QC/Console/Command/Argument: build: Argument of type [b]" + type + "[/b] isn't supported.");
			return FAILED;
		}

		return new Argument(name, type, description);
	}

	public static List<Argument> createAll(List<String> args) {
		List<Argument> builtArgs = new List<Argument>();

		Argument tempArg = null;
		foreach (var arg in args) {
			tempArg = null;

			match typeof(arg):
			TYPE_ARRAY:
			tempArg = create(arg[0], arg.size() > 1 ? arg[1] : 0);

			TYPE_STRING:
			tempArg = create(arg);

			TYPE_OBJECT, TYPE_INT:
			tempArg = create(null, arg);

			
			if (tempArg == null) {
				return null;
			}
			
			builtArgs.Add(tempArg);
		}
	} 
}
}