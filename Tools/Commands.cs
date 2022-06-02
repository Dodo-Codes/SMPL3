﻿namespace SMPL.Commands
{
	public abstract class Command
	{
		protected static readonly SortedDictionary<string, Command> commands = new();

		static Command()
		{
			AddCommand<CommandHelp>();
			AddCommand<CommandClear>();

			void AddCommand<T>() where T : Command
			{
				var command = (Command)Activator.CreateInstance<T>();
				commands.Add(command.GetName(), command);
			}
		}

		public abstract string GetName();
		public abstract string GetDescription();
		public abstract int GetParameterCount();
		public virtual string GetParameterName(int index) => $"param{index}";
		public abstract void Execute(string[] parameters);

		public static void TryExecute(string[] command)
		{
			if (command == null || command.Length == 0 || commands.ContainsKey(command[0]) == false)
				return;

			var cmd = commands[command[0]];
			var cmdArgCount = cmd.GetParameterCount();
			var args = new string[cmdArgCount];

			for (int i = 1; i < command.Length; i++)
			{
				if (args.Length <= i - 1)
					break;

				args[i - 1] = command[i];
			}
			cmd.Execute(args);
		}
	}
	public class CommandHelp : Command
	{
		public override string GetName() => "help";
		public override string GetDescription() => "Display all commands.";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters)
		{
			Debug.Log("Commands:", Color.White);
			foreach (var kvp in commands)
			{
				var cmd = kvp.Value;
				Debug.Log($"{kvp.Key} ", Color.Lime, false);
				for (int i = 0; i < cmd.GetParameterCount(); i++)
					Debug.Log($"[{cmd.GetParameterName(i)}] ", Color.Yellow, false);
				Debug.Log($"- {cmd.GetDescription()}", Color.Gray);
			}
		}
	}
	public class CommandClear : Command
	{
		public override string GetName() => "clear";
		public override string GetDescription() => "Clear the log.";
		public override int GetParameterCount() => 0;
		public override void Execute(string[] parameters) => SMPL.instance.ClearLog();
	}
}