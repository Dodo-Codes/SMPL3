namespace SMPL.Tools
{
	public static class Dodo
	{
		private readonly static Dictionary<Type, List<string>>
			allSetters = new(), allGetters = new(), allVoidMethods = new(), allReturnMethods = new();
		//private readonly static Dictionary<Type, List<Type>>
		//	setterTypes = new(), getterTypes = new(), returnMethodTypes = new();
		//private readonly static Dictionary<Type, List<Type[]>>
		//	returnMethodParamTypes = new(), voidMethodParamTypes = new();
		private readonly static Dictionary<(Type, string), MemberSetter> setters = new();
		private readonly static Dictionary<(Type, string), MemberGetter> getters = new();
		private readonly static Dictionary<(Type, string), Fasterflect.MethodInvoker>
			voidMethods = new(), returnMethods = new();

		public static void Set(string uniqueID, string propertyName, string value)
		{
			var obj = GetObject(uniqueID);
			var type = obj.GetType();
			var key = (type, propertyName);

			if(setters.ContainsKey(key) == false)
			{
				TryAddAllProps(type, true);
				if(allSetters[type].Contains(propertyName))
					setters[key] = type.DelegateForSetPropertyValue(propertyName);
				else
					MissingPropError(type, obj, propertyName, true);
			}

			if(setters.ContainsKey(key))
				setters[key].Invoke(obj, value);
		}
		public static object Get(object obj, string propertyName)
		{
			var type = obj.GetType();
			var key = (type, propertyName);

			if(getters.ContainsKey(key) == false)
			{
				TryAddAllProps(type, false);

				if(allGetters[type].Contains(propertyName))
					getters[key] = type.DelegateForGetPropertyValue(propertyName);
				else
					MissingPropError(type, obj, propertyName, false);
			}

			return getters.ContainsKey(key) ? getters[key].Invoke(obj) : default;
		}
		public static void Do(object obj, string methodName, params object[] parameters)
		{
			var type = obj.GetType();
			var key = (type, methodName);

			if(voidMethods.ContainsKey(key) == false)
			{
				TryAddAllMethods(type, true);

				if(allVoidMethods[type].Contains(methodName))
					voidMethods[key] = type.DelegateForCallMethod(methodName);
				else
					MissingMethodError(type, obj, methodName, true);
			}

			if(voidMethods.ContainsKey(key))
				voidMethods[key].Invoke(obj, parameters);
		}
		public static object DoGet(object obj, string methodName, params object[] parameters)
		{
			var type = obj.GetType();
			var key = (type, methodName);

			if(returnMethods.ContainsKey(key) == false)
			{
				TryAddAllMethods(type, false);

				if(allReturnMethods[type].Contains(methodName))
				{
					var p = type.GetMethod(methodName).GetParameters();
					var paramTypes = new List<Type>();
					for(int i = 0; i < p.Length; i++)
						paramTypes.Add(p[i].ParameterType);

					returnMethods[key] = type.DelegateForCallMethod(methodName, paramTypes.ToArray());
				}
				else
					MissingMethodError(type, obj, methodName, false);
			}

			return returnMethods.ContainsKey(key) ? returnMethods[key].Invoke(obj, parameters) : default;
		}

		private static void TryAddAllProps(Type type, bool setter)
		{
			var all = setter ? allSetters : allGetters;

			if(all.ContainsKey(type))
				all[type] = new();

			var props = type.GetProperties();
			var propNames = new List<string>();
			for(int i = 0; i < props.Length; i++)
			{
				if((props[i].CanWrite == false && setter) || (props[i].CanRead == false && setter == false))
					continue;

				var name = props[i].Name;
				propNames.Add(name);
			}
			all[type] = propNames;
		}
		private static void TryAddAllMethods(Type type, bool onlyVoid)
		{
			var all = onlyVoid ? allVoidMethods : allReturnMethods;

			if(all.ContainsKey(type))
				all[type] = new();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
			var methodNames = new List<string>();
			for(int i = 0; i < methods.Length; i++)
			{
				if(methods[i].IsSpecialName || methods[i].DeclaringType == typeof(object) ||
					methods[i].Name == "ToString" ||
					(methods[i].ReturnType != typeof(void) && onlyVoid) ||
					(methods[i].ReturnType == typeof(void) && onlyVoid == false))
					continue;

				var name = methods[i].Name;
				methodNames.Add(name);
			}
			all[type] = methodNames;
		}

		private static string GetAllProps(Type type, bool setter)
		{
			var all = setter ? allSetters[type] : allGetters[type];
			var result = "";

			for(int i = 0; i < all.Count; i++)
			{
				var sep = i == 0 ? "" : " ";
				result += $"{sep}[{all[i]}]";
			}

			return result;
		}
		private static string GetAllMethods(Type type, bool onlyVoid)
		{
			var all = onlyVoid ? allVoidMethods[type] : allReturnMethods[type];
			var result = "";

			for(int i = 0; i < all.Count; i++)
			{
				var sep = i == 0 ? "" : " ";
				result += $"{sep}<{all[i]}>";
			}

			return result;
		}

		private static void MissingPropError(Type type, object obj, string propertyName, bool setter)
		{
			var set = setter ? "set" : "get";
			Debug.LogError($"{{{obj}}} does not have [{propertyName}].",
						$"List of all [{set} variables] of {{{obj}}}: '{GetAllProps(type, setter)}'.");
		}
		private static void MissingMethodError(Type type, object obj, string methodName, bool isVoid)
		{
			var v = isVoid ? "do" : "doGet";
			Debug.LogError($"{{{obj}}} does not have <{methodName}>.",
				$"List of all <{v} actions> of {{{obj}}}: '{GetAllMethods(type, isVoid)}'.");
		}

		private static object GetObject(string uniqueID)
		{
			var obj = Scene.CurrentScene.Pick(uniqueID);

			if(obj == null)
			{
				Debug.LogError($"No {{object}} with [{nameof(obj.UniqueID)}] '{uniqueID}' exists.");
				if(Scene.CurrentScene.objsUID.Count > 0)
					Command.commands["objects"].Execute(null);
				return default;
			}
			return obj;
		}
		private static object ParseValue(string value)
		{
			return 0;
		}
	}
}
