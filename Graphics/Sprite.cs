﻿namespace SMPL.Graphics
{
	public class Sprite : Visual
	{
		public Vector2 TexCoordsUnitA { get; set; }
		public Vector2 TexCoordsUnitB { get; set; } = new(1, 1);

		public Vector2 LocalSize { get; set; } = new(100, 100);
		public Vector2 Size
		{
			get => LocalSize * GetArea().Scale;
			set => LocalSize = value / GetArea().Scale;
		}

		public Vector2 OriginUnit { get; set; } = new(0.5f, 0.5f);
		public Vector2 Origin
		{
			get => OriginUnit * LocalSize;
			set => OriginUnit = value / LocalSize;
		}

		public Vector2 CornerA => GetArea().GetPositionFromSelf(-Origin);
		public Vector2 CornerB => GetArea().GetPositionFromSelf(new Vector2(LocalSize.X, 0) - Origin);
		public Vector2 CornerC => GetArea().GetPositionFromSelf(LocalSize - Origin);
		public Vector2 CornerD => GetArea().GetPositionFromSelf(new Vector2(0, LocalSize.Y) - Origin);

		public Sprite()
		{
			UniqueID = $"{nameof(SMPL)} {nameof(Sprite)}";
		}

		protected override void OnDraw()
		{
			if(IsHidden)
				return;

			var camera = DrawTarget;
			camera ??= Scene.MainCamera;

			var w = Texture == null ? 0 : Texture.Size.X;
			var h = Texture == null ? 0 : Texture.Size.Y;
			var w0 = w * TexCoordsUnitA.X;
			var ww = w * TexCoordsUnitB.X;
			var h0 = h * TexCoordsUnitA.Y;
			var hh = h * TexCoordsUnitB.Y;
			var tint = Tint.ToSFML();

			var verts = new Vertex[]
			{
				new(CornerA.ToSFML(), tint, new(w0, h0)),
				new(CornerB.ToSFML(), tint, new(ww, h0)),
				new(CornerC.ToSFML(), tint, new(ww, hh)),
				new(CornerD.ToSFML(), tint, new(w0, hh)),
			};

			camera.renderTexture.Draw(verts, PrimitiveType.Quads, new(BlendMode, Transform.Identity, Texture, Shader));
		}

		private Area GetArea()
		{
			if(HasPart<Area>() == false)
				SetPart(new Area());

			return GetPart<Area>();
		}
	}
}