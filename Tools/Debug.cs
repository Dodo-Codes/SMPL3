using System.Diagnostics;

namespace SMPL.Tools
{
	public static class Debug
	{
		public static uint CallChainIndex { get; set; }
		public static int LineNumber => new StackFrame((int)CallChainIndex + 1, true)?.GetFileLineNumber() ?? 0;
		public static string MethodName
		{
			get
			{
				var method = new StackFrame((int)CallChainIndex + 1, true)?.GetMethod();
				if(method == null)
					return default;

				var name = method.Name;
				name = name.Replace(".ctor", $"new {method.DeclaringType.Name}");
				name = name.Replace("g__", "");

				var split = name.Split("|");
				if(split.Length > 1)
					name = split[0];
				var split2 = name.Split(">");
				if(split2.Length > 1)
					name = split2[1];

				return name;
			}
		}
		public static string ClassName => new StackFrame((int)CallChainIndex + 1, true)?.GetMethod()?.DeclaringType?.Name;
		public static string Namespace => new StackFrame((int)CallChainIndex + 1, true)?.GetMethod()?.DeclaringType?.Namespace;
		public static string FilePath => new StackFrame((int)CallChainIndex + 1, true)?.GetFileName();
		public static string FileName => Path.GetFileNameWithoutExtension(new StackFrame((int)CallChainIndex + 1, true)?.GetFileName());
		public static string FileDirectory => Path.GetDirectoryName(new StackFrame((int)CallChainIndex + 1, true)?.GetFileName());

		public static void Log(object message, Color color = default, bool newLine = true)
		{
			SMPL.instance.Log(message, color, newLine);
		}
		public static void LogError(object message, string tip = default, int callChainIndex = 1)
		{
			if(Debugger.IsAttached == false || callChainIndex < -1)
				return;

			//var methods = new List<string>();
			//var actions = new List<string>();

			Log("");
			//if (callChainIndex >= 0)
			//	for (int i = 0; i < 50; i++)
			//		Add(callChainIndex + i + 1);
			//
			//for (int i = methods.Count - 1; i >= 0; i--)
			//{
			//	Log("[!] ", Color.DarkRed, false);
			//	Log("- ", i == 0 ? Color.White : Color.Gray, false);
			//	Log($"{methods[i]}", i == 0 ? Color.White : Color.Gray, false);
			//	Log($"{actions[i]}", i == 0 ? Color.Red : Color.Gray);
			//}
			Log("[!] Error: ", Color.DarkRed, false);
			Log($"{message}", Color.Red);
			if(string.IsNullOrWhiteSpace(tip) == false)
			{
				Log($"[?] Tip: ", Color.DarkCyan, false);
				Log($"{tip}", Color.Cyan);
			}

			//void Add(int depth)
			//{
			//	if(depth < 0)
			//		return;
			//
			//	var prevDepth = CallChainIndex;
			//	CallChainIndex = (uint)depth;
			//	var action = MethodName;
			//	if(string.IsNullOrEmpty(action))
			//		return;
			//
			//
			//	CallChainIndex = (uint)depth + 1;
			//	var file = FileName;
			//	if(file != null)
			//	{
			//		methods.Add($"{file}.cs/{MethodName}()");
			//		actions.Add($" {{ [{LineNumber}] {action}(); }}");
			//	}
			//	CallChainIndex = prevDepth;
			//}
		}
	}
}
