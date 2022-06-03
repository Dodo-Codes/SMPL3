namespace SMPL.Tools
{
	public class Hitbox : Part
	{
		public List<Line> LocalLines { get; } = new();
		public List<Line> Lines { get; } = new();
		[JsonIgnore]
		public bool IsHovered => ConvexContains(Scene.MouseCursorPosition);

		protected override void OnUpdate()
		{
			if(Owner == null || Owner.HasPart<Area>() == false)
				return;

			var area = Owner.GetPart<Area>();
			Lines.Clear();

			for(int i = 0; i < LocalLines.Count; i++)
			{
				var a = area.GetPositionFromSelf(LocalLines[i].A);
				var b = area.GetPositionFromSelf(LocalLines[i].B);
				Lines.Add(new(a, b));
			}
		}

		public void AddLineSegmentLocal(params Vector2[] localPoints)
		{
			AddLineSegment(localPoints, LocalLines);
		}
		public void AddLineSegment(params Vector2[] points)
		{
			AddLineSegment(points, Lines);
		}

		public void Draw(Camera camera = default, Color color = default, float width = 4)
		{
			camera ??= Scene.MainCamera;
			color = color == default ? Color.White : color;

			for(int i = 0; i < Lines.Count; i++)
				Lines[i].Draw(camera, color, width);
		}
		public List<Vector2> GetCrossPoints(Hitbox hitbox)
		{
			var result = new List<Vector2>();
			for(int i = 0; i < Lines.Count; i++)
				for(int j = 0; j < hitbox.Lines.Count; j++)
				{
					var p = Lines[i].GetCrossPoint(hitbox.Lines[j]);
					if(p.IsNaN() == false)
						result.Add(p);
				}
			return result;
		}
		public bool ConvexOverlaps(Hitbox hitbox)
		{
			return Crosses(hitbox) || ConvexContains(hitbox);
		}
		public bool ConvexContains(Vector2 point)
		{
			if(Lines == null || Lines.Count < 3)
				return false;

			var crosses = 0;
			var outsidePoint = Lines[0].A.PointPercentTowardPoint(Lines[0].B, new(-500, -500));

			for(int i = 0; i < Lines.Count; i++)
				if(Lines[i].Crosses(new(point, outsidePoint)))
					crosses++;

			return crosses % 2 == 1;
		}
		public bool Crosses(Hitbox hitbox)
		{
			for(int i = 0; i < Lines.Count; i++)
				for(int j = 0; j < hitbox.Lines.Count; j++)
					if(Lines[i].Crosses(hitbox.Lines[j]))
						return true;

			return false;
		}
		public bool ConvexContains(Hitbox hitbox)
		{
			for(int i = 0; i < hitbox.Lines.Count; i++)
				for(int j = 0; j < Lines.Count; j++)
					if((ConvexContains(hitbox.Lines[i].A) == false || ConvexContains(hitbox.Lines[i].B) == false) &&
						(hitbox.ConvexContains(Lines[j].A) == false || hitbox.ConvexContains(Lines[j].B) == false))
						return false;
			return true;
		}
		public Vector2 GetClosestPoint(Vector2 point)
		{
			var points = new List<Vector2>();
			var result = new Vector2();
			var bestDist = float.MaxValue;

			for(int i = 0; i < Lines.Count; i++)
				points.Add(Lines[i].GetClosestPoint(point));
			for(int i = 0; i < points.Count; i++)
			{
				var dist = points[i].DistanceBetweenPoints(point);
				if(dist < bestDist)
				{
					bestDist = dist;
					result = points[i];
				}
			}
			return result;
		}

		private static void AddLineSegment(Vector2[] points, List<Line> list)
		{
			if(points == null || points.Length < 2)
				return;

			for(int i = 1; i < points.Length; i++)
				list.Add(new(points[i - 1], points[i]));
		}
	}
}
