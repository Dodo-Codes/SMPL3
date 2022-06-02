namespace SMPL
{
	public abstract class Part
	{
		internal readonly List<Type> required = new();

		public BaseObject Owner { get; internal set; }

		protected void AddRequirement<T>() where T : Part
		{
			var type = typeof(T);

			if (required.Contains(type) == false)
				required.Add(type);
		}
		protected bool Requires<T>() where T : Part
		{
			return required.Contains(typeof(T));
		}

		protected virtual void Initialize() { }
		protected virtual void Update() { }
		protected virtual void Destroy() { }
	}
}
