namespace SMPL
{
	public abstract class Part
	{
		internal readonly List<Type> required = new();

		public BaseObject Owner { get; internal set; }

		protected void AddRequirement<T>() where T : Part
		{
			Add(typeof(T));
		}
		protected void AddRequirement(Part partType)
		{
			Add(partType.GetType());
		}
		private void Add(Type partType)
		{
			if (required.Contains(partType) == false)
				required.Add(partType);
		}

		protected bool Requires<T>() where T : Part
		{
			return required.Contains(typeof(T));
		}
		protected bool Requires(Type partType)
		{
			return required.Contains(partType);
		}

		protected virtual void OnCreate() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnDestroy() { }

		internal void Initialize() => OnCreate();
		internal void Update() => OnUpdate();
		internal void Destroy() => OnDestroy();
	}
}
