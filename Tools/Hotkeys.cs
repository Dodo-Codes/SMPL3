using static SFML.Window.Keyboard;

namespace SMPL.Tools
{
	public abstract class Hotkey
	{
		internal static readonly List<Hotkey> hotkeys = new();

		static Hotkey()
		{
			var type = typeof(Hotkey);
			var assembly = Assembly.GetAssembly(type);
			var types = assembly.GetTypes();

			for(int i = 0; i < types.Length; i++)
				if(types[i].IsSubclassOf(type))
				{
					var hotkey = (Hotkey)Activator.CreateInstance(types[i]);
					hotkeys.Add(hotkey);
				}
		}

		private void TryTrigger()
		{
			var keys = GetKeys();
			if(Pressed(keys.Item1) && Pressed(keys.Item2) && Pressed(keys.Item3))
				Trigger();

			bool Pressed(Key key) => key == Key.Unknown || IsKeyPressed(key);
		}

		public string GetName()
		{
			var result = "UnknownKey";
			var keys = GetKeys();

			if(keys.Item1 != Key.Unknown)
				result = $"{keys.Item1}";

			if(keys.Item2 != Key.Unknown)
				result += $" + {keys.Item2}";

			if(keys.Item3 != Key.Unknown)
				result += $" + {keys.Item3}";

			return result;
		}
		public abstract string GetDescription();
		public abstract (Key, Key, Key) GetKeys();
		public abstract void Trigger();

		public static void TryTriggerHotkeys()
		{
			for(int i = 0; i < hotkeys.Count; i++)
				hotkeys[i].TryTrigger();
		}
	}
	public class HotkeyUndo : Hotkey
	{
		public override string GetDescription() => "Undo the last scene action.";
		public override (Key, Key, Key) GetKeys() => (Key.LControl, Key.Z, Key.Unknown);
		public override void Trigger() => Debug.Log("undo");
	}
	public class HotkeySave : Hotkey
	{
		public override string GetDescription() => "Save the scene.";
		public override (Key, Key, Key) GetKeys() => (Key.LControl, Key.S, Key.Unknown);
		public override void Trigger() => Debug.Log("save");
	}
}
