using System;
using System.Collections.Generic;
using Godotcraft.scripts.objects.console.type;
using String = System.String;

namespace Godotcraft.scripts.objects.console.argument {
public class ArgumentFactory {

	public static Argument create(String name, Type type, String description = null) {
		// Define argument type
			BaseType basetype = TypeFactory.create(type);

		if (type is BaseType) {
			Console.Log.error("QC/Console/Command/Argument: build: Argument of type [b]" + type + "[/b] isn't supported.");
			return null;
		}

		return new Argument(name, basetype, description);
	}

	public static List<Argument> createAll(List<String> args) {
		List<Argument> builtArgs = new List<Argument>();

		// Argument tempArg = null;
		// foreach (var arg in args) {
		// 	tempArg = null;
		//
		// 	match typeof(arg):
		// 	TYPE_ARRAY:
		// 	tempArg = create(arg[0], arg.size() > 1 ? arg[1] : 0);
		//
		// 	TYPE_STRING:
		// 	tempArg = create(arg);
		//
		// 	TYPE_OBJECT, TYPE_INT:
		// 	tempArg = create(null, arg);
		//
		// 	
		// 	if (tempArg == null) {
		// 		return null;
		// 	}
		// 	
		// 	builtArgs.Add(tempArg);
		// }

		return builtArgs;
	} 
}
}