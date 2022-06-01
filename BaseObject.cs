using SMPL.Tools;

namespace SMPL
{
	public class BaseObject
	{
		private readonly Dictionary<Type, Part> parts = new();

		public void SetPart(Part part)
		{
			var fail = false;
			var type = part.GetType();
			for (int i = 0; i < part.required.Count; i++)
				if (HasPart(part.required[i]) == false)
				{
					Debug.LogError($"The {nameof(Part)} '{type.Name}' cannot be set in this {nameof(BaseObject)} " +
						$"since it requires the {nameof(Part)} '{part.required[i].Name}'.",
						$"To resolve that, set the required '{part.required[i].Name}' {nameof(Part)} beforehand.");
					fail = true;
				}

			if (fail == false)
			{
				part.Owner = this;
				parts[type] = part;
				part.Initialize();
			}
		}
		public T GetPart<T>() where T : Part
		{
			if (HasPart<T>() == false)
			{
				Debug.LogError($"No {nameof(Part)} '{typeof(T).Name}' was found on that {nameof(BaseObject)}.");
				return default;
			}
			return (T)parts[typeof(T)];
		}
		public bool HasPart(Type partType)
		{
			return parts.ContainsKey(partType);
		}
		public bool HasPart<T>() where T : Part
		{
			return parts.ContainsKey(typeof(T));
		}
		public void RemovePart<T>() where T : Part
		{
			var type = typeof(T);
			if (parts.ContainsKey(type) == false)
				return;

			parts[type].Owner = null;
			parts.Remove(type);
		}
		public void RemoveAllParts()
		{
			foreach (var kvp in parts)
				kvp.Value.Owner = null;

			parts.Clear();
		}

		public virtual void Update()
		{
			foreach (var kvp in parts)
				kvp.Value.Update();
		}
		public virtual void Destroy()
		{
			RemoveAllParts();
		}
	}
}
