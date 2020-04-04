using System;
using System.Collections.Generic;

namespace Godotcraft.scripts.objects.console.type {
public class TypeFactory {
	
	private static List<Type> typeList = new List<Type>();

	static TypeFactory(){
		typeList.Add(typeof(Any));
		typeList.Add(typeof(Bool));
		typeList.Add(typeof(Filter));
		typeList.Add(typeof(Float));
		typeList.Add(typeof(Int));
		typeList.Add(typeof(String));
	}

	public static BaseType create(Type type) {
		return (BaseType) Activator.CreateInstance(type);
	}
}
}