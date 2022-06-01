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
		public bool Requires<T>() where T : Part
		{
			return required.Contains(typeof(T));
		}

		public virtual void Initialize() { }
		public virtual void Update() { }
		public virtual void Destroy() { }
	}
}
