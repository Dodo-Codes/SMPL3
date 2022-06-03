using System.Collections.ObjectModel;

namespace SMPL.Parts
{
	public class Area : Part
	{
		private Vector2 localPos;
		private float localAng, localSc;
		private Matrix3x2 global;

		public Vector2 LocalDirection
		{
			get => Vector2.Normalize(LocalAngle.AngleToDirection());
			set => LocalAngle = Vector2.Normalize(value).DirectionToAngle();
		}
		public Vector2 Direction
		{
			get => Vector2.Normalize(Angle.AngleToDirection());
			set => Angle = Vector2.Normalize(value).DirectionToAngle();
		}

		public Vector2 LocalPosition
		{
			get => localPos;
			set { localPos = value; UpdateSelfAndChildren(); }
		}
		public float LocalScale
		{
			get => localSc;
			set { localSc = value; UpdateSelfAndChildren(); }
		}
		public float LocalAngle
		{
			get => localAng;
			set { localAng = value; UpdateSelfAndChildren(); }
		}

		public Vector2 Position
		{
			get => GetPosition(global);
			set => LocalPosition = GetLocalPositionFromParent(value);
		}
		public float Scale
		{
			get => GetScale(global);
			set => LocalScale = GetScale(GlobalToLocal(value, Angle, Position));
		}
		public float Angle
		{
			get => GetAngle(global);
			set => LocalAngle = GetAngle(GlobalToLocal(Scale, value, Position));
		}

		public Vector2 GetLocalPositionFromParent(Vector2 position)
		{
			return GetPosition(GlobalToLocal(Scale, Angle, position));
		}
		public Vector2 GetPositionFromParent(Vector2 localPosition)
		{
			return GetPosition(LocalToGlobal(LocalScale, LocalAngle, localPosition));
		}
		public Vector2 GetLocalPositionFromSelf(Vector2 position)
		{
			var m = Matrix3x2.Identity;
			m *= Matrix3x2.CreateTranslation(position);
			m *= Matrix3x2.CreateTranslation(Position);

			return GetPosition(m);
		}
		public Vector2 GetPositionFromSelf(Vector2 localPosition)
		{
			var m = Matrix3x2.Identity;
			m *= Matrix3x2.CreateTranslation(localPosition);
			m *= Matrix3x2.CreateRotation(LocalAngle.DegreesToRadians());
			m *= Matrix3x2.CreateScale(LocalScale);
			m *= Matrix3x2.CreateTranslation(LocalPosition);

			return GetPosition(m * GetParentMatrix());
		}

		protected override void OnCreate()
		{
			LocalScale = 1;
		}

		private void UpdateSelfAndChildren()
		{
			global = LocalToGlobal(LocalScale, LocalAngle, LocalPosition);

			var children = GetFamily().Children;
			for (int i = 0; i < children.Count; i++)
				children[i].Owner.GetPart<Area>().UpdateSelfAndChildren();
		}

		internal Matrix3x2 LocalToGlobal(float localScale, float localAngle, Vector2 localPosition)
		{
			var c = Matrix3x2.Identity;
			c *= Matrix3x2.CreateScale(localScale);
			c *= Matrix3x2.CreateRotation(localAngle.DegreesToRadians());
			c *= Matrix3x2.CreateTranslation(localPosition);

			return c * GetParentMatrix();
		}
		internal Matrix3x2 GlobalToLocal(float scale, float angle, Vector2 position)
		{
			var c = Matrix3x2.Identity;
			c *= Matrix3x2.CreateScale(scale);
			c *= Matrix3x2.CreateRotation(angle.DegreesToRadians());
			c *= Matrix3x2.CreateTranslation(position);

			return c * GetInverseParentMatrix();
		}
		internal Matrix3x2 GetParentMatrix()
		{
			var p = Matrix3x2.Identity;
			var parent = GetFamily()?.Parent?.GetArea();
			if (parent != null)
			{
				p *= Matrix3x2.CreateScale(parent.Scale);
				p *= Matrix3x2.CreateRotation(parent.Angle.DegreesToRadians());
				p *= Matrix3x2.CreateTranslation(parent.Position);
			}
			return p;
		}
		internal Matrix3x2 GetInverseParentMatrix()
		{
			var inverseParent = Matrix3x2.Identity;
			var parent = GetFamily()?.Parent?.GetArea();
			if (parent != null)
			{
				Matrix3x2.Invert(Matrix3x2.CreateScale(parent.Scale), out var s);
				Matrix3x2.Invert(Matrix3x2.CreateRotation(parent.Angle.DegreesToRadians()), out var r);
				Matrix3x2.Invert(Matrix3x2.CreateTranslation(parent.Position), out var t);

				inverseParent *= t;
				inverseParent *= r;
				inverseParent *= s;
			}

			return inverseParent;
		}

		internal static float GetAngle(Matrix3x2 matrix)
		{
			return MathF.Atan2(matrix.M12, matrix.M11).RadiansToDegrees();
		}
		internal static Vector2 GetPosition(Matrix3x2 matrix)
		{
			return new(matrix.M31, matrix.M32);
		}
		internal static float GetScale(Matrix3x2 matrix)
		{
			return MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12);
			//return new(MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12),
			//	MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22));
		}

		internal Family GetFamily()
		{
			if (Owner.HasPart<Family>() == false)
				Owner.SetPart(new Family());

			return Owner.GetPart<Family>();
		}
	}
}
