using System.Collections.ObjectModel;

namespace SMPL.Parts
{
	public class Family : Part
	{
		private List<Family> children = new();
		private Family parent;

		[JsonIgnore]
		public Family Parent
		{
			get => parent;
			set
			{
				if (parent == value)
					return;

				if (parent != value && parent != null && parent.children != null)
					parent.children.Remove(this);

				var area = GetArea();
				var prevPos = area.Position;
				var prevAng = area.Angle;
				var prevSc = area.Scale;

				parent = value;

				if (parent != null)
					parent.children.Add(this);

				area.Position = prevPos;
				area.Angle = prevAng;
				area.Scale = prevSc;
			}
		}
		[JsonIgnore]
		public ReadOnlyCollection<Family> Children => children.AsReadOnly();

		protected override void OnDestroy()
		{
			Parent = null;
			for (int i = 0; i < children.Count; i++)
				children[i].Parent = null;
			children = null;
		}

		internal Area GetArea()
		{
			if (Owner.HasPart<Area>() == false)
				Owner.SetPart(new Area());

			return Owner.GetPart<Area>();
		}
	}
}
