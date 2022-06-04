namespace SMPL.Commands
{
	public abstract class Command
	{
		protected static readonly SortedDictionary<string, Command> commands = new();

		static Command()
		{
			var type = typeof(Command);
			var assembly = Assembly.GetAssembly(type);
			var types = assembly.GetTypes();

			for(int i = 0; i < types.Length; i++)
				if(types[i].IsSubclassOf(type))
				{
					var command = (Command)Activator.CreateInstance(types[i]);
					commands.Add(command.GetName(), command);
				}
		}

		public abstract string GetName();
		public abstract string GetDescription();
		public abstract int GetParameterCount();
		public virtual string GetParameterName(int index) => $"param{index}";
		public abstract void Execute(string[] parameters);
		public void Display(bool newLine = true)
		{
			Debug.Log($"{GetName()} ", Color.Lime, false);
			for(int i = 0; i < GetParameterCount(); i++)
				Debug.Log($"{GetParameterName(i)} ", Color.Yellow, false);
			Debug.Log($"- {GetDescription()}", Color.Gray, newLine);
		}

		public static void TryExecute(string[] command)
		{
			if(command == null || command.Length == 0 || commands.ContainsKey(command[0]) == false)
				return;

			var cmd = commands[command[0]];
			var cmdArgCount = cmd.GetParameterCount();
			var args = new string[cmdArgCount];

			for(int i = 1; i < command.Length; i++)
			{
				if(args.Length <= i - 1)
					break;

				args[i - 1] = command[i];
			}
			cmd.Execute(args);
		}
	}
	public class CommandCommands : Command
	{
		public override string GetName() => "commands";
		public override string GetDescription() => "Display all commands.";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters)
		{
			Debug.Log("\nCommands:");
			foreach(var kvp in commands)
				kvp.Value.Display();
		}
	}
	public class CommandClear : Command
	{
		public override string GetName() => "clear";
		public override string GetDescription() => "Clear the log.";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters) => SMPL.instance.ClearLog();
	}
	public class CommandHotkeys : Command
	{
		public override string GetName() => "hotkeys";
		public override string GetDescription() => "Display all hotkeys.";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters)
		{
			var hotkeys = Hotkey.hotkeys;
			Debug.Log("");
			Debug.Log("Hotkeys:");
			for(int i = 0; i < hotkeys.Count; i++)
			{
				var hotkey = hotkeys[i];
				Debug.Log(hotkey.GetName(), Color.Lime, false);
				Debug.Log($" - {hotkey.GetDescription()}", Color.Gray);
			}
		}
	}
	public class CommandUndo : Command
	{
		internal static List<string> actions = new();

		public override string GetName() => "undo";
		public override string GetDescription() => $"Undo the last '{GetParameterName(0)}' of scene actions.";
		public override int GetParameterCount() => 1;
		public override string GetParameterName(int index) => "amount";
		public override void Execute(string[] parameters)
		{

		}
	}
	public class CommandChangeValue : Command
	{
		public override string GetName() => "value";
		public override string GetDescription()
			=> $"Change {GetParameterName(0)}'s [{GetParameterName(1)}] to '{GetParameterName(2)}'.";
		public override int GetParameterCount() => 3;
		public override string GetParameterName(int index)
			=> new string[] { "uniqueID", "variable", "newValue" }[index];
		public override void Execute(string[] parameters)
		{
			var obj = Scene.CurrentScene.Pick(parameters[0]);

			if(obj == null)
			{
				DisplayCommand();
				Debug.LogError($"No {{object}} with [{nameof(obj.UniqueID)}] '{parameters[0]}' exists.");
				if(Scene.CurrentScene.objsUID.Count > 0)
					commands["objects"].Execute(null);
				return;
			}

			var props = obj.GetType().GetProperties();
			var prop = default(PropertyInfo);

			if(parameters[1] != null)
				for(int i = 0; i < props.Length; i++)
					if(props[i].CanWrite && props[i].Name.ToLower() == parameters[1].ToLower())
						prop = props[i];

			if(prop == null)
			{
				var propNames = "";
				for(int i = 0; i < props.Length; i++)
				{
					if(props[i].CanWrite == false)
						continue;

					var name = props[i].Name;
					var sep = i == 0 ? "" : " ";
					propNames += $"{sep}[{name}]";
				}
				var list = $"List of all [variables] of {{{parameters[0]}}}: '{propNames}'.";

				if(parameters[1] == null)
				{
					Debug.Log($"\n[?] {list}", Color.Cyan);
					return;
				}

				DisplayCommand();
				Debug.LogError($"{{{parameters[0]}}} does not have [{parameters[1]}].", list);
				return;
			}

			var isInt = prop.PropertyType == typeof(int);
			var isFloat = prop.PropertyType == typeof(float);
			var isVec = prop.PropertyType == typeof(Vector2);
			var isCol = prop.PropertyType == typeof(Color);

			var numb = parameters[2].ToNumber();
			var values = parameters[2]?.Split(',');

			if(values == null)
			{
				Debug.Log($"\n[?] The [{prop.Name}] of {{{parameters[0]}}} is '{prop.GetValue(obj)}'.", Color.Cyan);
				return;
			}

			var vec = new Vector2(GetNumber(isVec, 2, 0), GetNumber(isVec, 2, 1));
			var col1 = new Vector2(GetNumber(isCol, 4, 0).Limit(0, 255), GetNumber(isCol, 4, 1).Limit(0, 255));
			var col2 = new Vector2(GetNumber(isCol, 4, 2).Limit(0, 255), GetNumber(isCol, 4, 3).Limit(0, 255));

			var isInvalidInt = isInt && float.IsNaN(numb);
			var isInvalidFloat = isFloat && float.IsNaN(numb);
			var isInvalidVec = isVec && vec.IsNaN();
			var isInvalidCol = isCol && (col1.IsNaN() || col2.IsNaN());

			if(isInvalidInt || isInvalidFloat || isInvalidVec || isInvalidCol)
			{
				var moreDetail = "";

				if(isInvalidFloat)
					moreDetail = "It expects a number.";
				else if(isInvalidInt)
					moreDetail = "It expects a whole number.";
				else if(isInvalidVec)
					moreDetail = "It expects two numbers (x, y), separated by ','. For example: '69,420'.";
				else if(isInvalidCol)
					moreDetail = "It expects four numbers (red, green, blue, alpha), " +
						"separated by ','. For example: '10,140,240,255'.";

				DisplayCommand();
				Debug.LogError($"The [{prop.Name}] of {{{parameters[0]}}} " +
					$"does not understand the value '{parameters[2]}'.", moreDetail);
				return;
			}

			if(isFloat)
				prop.SetValue(obj, numb);
			else if(isInt)
				prop.SetValue(obj, (int)numb);
			else if(isVec)
				prop.SetValue(obj, vec);
			else if(isCol)
				prop.SetValue(obj, Color.FromArgb((byte)col2.Y, (byte)col1.X, (byte)col1.Y, (byte)col2.X));
			else // string
				prop.SetValue(obj, parameters[2]);

			float GetNumber(bool typeCondition, int amount, int index)
			{
				return typeCondition && values != null && values.Length == amount ?
					values[index].ToNumber() : float.NaN;
			}
			void DisplayCommand()
			{
				Debug.Log("\nCommand: ", Color.Gray, false);
				Display(false);
			}
		}
	}
	public class CommandObjects : Command
	{
		internal static List<string> actions = new();

		public override string GetName() => "objects";
		public override string GetDescription() => "Display all {objects} in the scene (by their [UniqueID]).";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters)
		{
			var objs = "";
			var i = 0;
			foreach(var kvp in Scene.CurrentScene.objsUID)
			{
				var sep = i == 0 ? "" : " ";
				objs += $"{sep}{{{kvp.Value.UniqueID}}}";
				i++;
			}
			var result = objs == "" ? "None" : $"'{objs}'";
			Debug.Log($"\n[?] List of all {{objects}} in this scene (by their [UniqueID]): {result}.", Color.Cyan);
		}
	}
}
