namespace SMPL
{
	public abstract class BaseObject
	{
		private string uid;
		private int order;
		private readonly Dictionary<Type, Part> parts = new();

		public string UniqueID
		{
			get => uid;
			set
			{
				value = value.Trim();

				if(string.IsNullOrWhiteSpace(value))
					value = "empty";

				var i = 1;
				var freeUID = value;
				var objsUID = Scene.CurrentScene.objsUID;

				while(objsUID.ContainsKey(freeUID))
				{
					freeUID = $"{value}{i}";
					i++;
				}

				if(uid != null)
					objsUID.Remove(uid);

				uid = freeUID;
				objsUID[uid] = this;
			}
		}
		public int UpdateOrder
		{
			get => order;
			set
			{
				var objsOrder = Scene.CurrentScene.objsOrder;

				if(objsOrder.ContainsKey(order) == false)
					objsOrder[order] = new();

				if(objsOrder[order].Contains(this))
					objsOrder[order].Remove(this);

				order = value;
				objsOrder[order].Add(this);
			}
		}

		public BaseObject()
		{
			UpdateOrder = 0;
		}

		public void SetPart(Part part)
		{
			var fail = false;
			var type = part.GetType();
			for(int i = 0; i < part.required.Count; i++)
				if(HasPart(part.required[i]) == false)
				{
					Debug.LogError($"The {nameof(Part)} '{type.Name}' cannot be set in this {nameof(BaseObject)} " +
						$"since it requires the {nameof(Part)} '{part.required[i].Name}'.",
						$"To resolve that, set the required '{part.required[i].Name}' {nameof(Part)} beforehand.");
					fail = true;
				}

			if(fail == false)
			{
				part.Owner = this;
				parts[type] = part;
				part.Initialize();
			}
		}

		public T GetPart<T>() where T : Part
		{
			return Get<T>(typeof(T));
		}
		public T GetPart<T>(Type partType) where T : Part
		{
			return Get<T>(partType);
		}
		private T Get<T>(Type partType) where T : Part
		{
			if(HasPart(partType) == false)
			{
				Debug.LogError($"No {nameof(Part)} '{typeof(T).Name}' was found on that {nameof(BaseObject)}.");
				return default;
			}
			return (T)parts[partType];
		}

		public bool HasPart(Type partType)
		{
			return parts.ContainsKey(partType);
		}
		public bool HasPart<T>() where T : Part
		{
			return parts.ContainsKey(typeof(T));
		}

		public void RemovePart(Type partType)
		{
			Remove(partType);
		}
		public void RemovePart<T>() where T : Part
		{
			Remove(typeof(T));
		}
		private void Remove(Type partType)
		{
			if(parts.ContainsKey(partType) == false)
				return;

			parts[partType].Owner = null;
			parts.Remove(partType);
		}

		public void RemoveAllParts()
		{
			foreach(var kvp in parts)
				kvp.Value.Owner = null;

			parts.Clear();
		}

		public virtual void Update()
		{
			foreach(var kvp in parts)
				kvp.Value.Update();
		}
		public virtual void Destroy()
		{
			RemoveAllParts();
		}
	}
}
