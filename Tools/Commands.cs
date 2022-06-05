namespace SMPL.Commands
{
	public abstract class Command
	{
		public static readonly SortedDictionary<string, Command> commands = new();

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
			Dodo.Set(parameters[0], parameters[1], parameters[2]);
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
