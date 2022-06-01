﻿using SFML.Graphics;
using System.Numerics;
using System;
using SFML.System;
using SFML.Window;
using SMPL.Graphics;
using SMPL.Tools;

namespace SMPL.Graphics
{
	public class Camera : BaseObject
	{
		internal RenderTexture renderTexture;

		public RenderTexture RenderTexture => renderTexture;
		public Vector2 Resolution { get; private set; }

		[JsonIgnore]
		public Texture Texture => renderTexture.Texture;

		public Vector2 Size => Resolution / GetArea().Scale;
		public Vector2 MouseCursorPosition
		{
			get { var p = Mouse.GetPosition(SMPL.scene); return PointToCamera(new(p.X, p.Y)); }
			set { var p = PointToWorld(value); Mouse.SetPosition(new((int)p.X, (int)p.Y), SMPL.scene); }
		}

		public Vector2 CornerA
		{
			get => GetArea().GetPositionFromSelf(new(
				-renderTexture.GetView().Size.X * 0.5f, -renderTexture.GetView().Size.Y * 0.5f));
		}
		public Vector2 CornerB
		{
			get => GetArea().GetPositionFromSelf(new(
				renderTexture.GetView().Size.X * 0.5f, -renderTexture.GetView().Size.Y * 0.5f));
		}
		public Vector2 CornerC
		{
			get => GetArea().GetPositionFromSelf(new(
				renderTexture.GetView().Size.X * 0.5f, renderTexture.GetView().Size.Y * 0.5f));
		}
		public Vector2 CornerD
		{
			get => GetArea().GetPositionFromSelf(new(
				-renderTexture.GetView().Size.X * 0.5f, renderTexture.GetView().Size.Y * 0.5f));
		}

		public Camera(Vector2 resolution)
		{
			Init((uint)resolution.X.Limit(0, Texture.MaximumSize), (uint)resolution.Y.Limit(0, Texture.MaximumSize));
		}
		public Camera(uint resolutionX, uint resolutionY)
		{
			Init(resolutionX, resolutionY);
		}

		public override void Destroy()
		{
			renderTexture.Dispose();
		}
		//public bool Captures(Hitbox hitbox)
		//{
		//	var screen = new Hitbox(CornerA, CornerB, CornerC, CornerD, CornerA);
		//	return screen.ConvexContains(hitbox);
		//}
		//public bool Captures(Vector2 point)
		//{
		//	var screen = new Hitbox(CornerA, CornerB, CornerC, CornerD, CornerA);
		//	return screen.ConvexContains(point);
		//}
		public void Snap(string imagePath)
		{
			var img = Texture.CopyToImage();
			var result = img.SaveToFile(imagePath);
			img.Dispose();

			if (result == false)
				Debug.LogError(1, $"Could not save the image at '{imagePath}'.");
		}
		public void Fill(SFML.Graphics.Color color = default)
		{
			renderTexture.Clear(color);
		}
		public override void Update()
		{
			base.Update();
			var view = RenderTexture.GetView();
			var area = GetArea();
			view.Center = area.Position.ToSFML();
			view.Rotation = area.Angle;
			view.Size = (Resolution * area.Scale).ToSFML();
			RenderTexture.SetView(view);
		}
		public void Display()
		{
			renderTexture.Display();
		}

		internal Vector2 PointToCamera(Vector2 worldPoint)
		{
			return SMPL.scene.MapPixelToCoords(new((int)worldPoint.X, (int)worldPoint.Y), renderTexture.GetView()).ToSystem();
		}
		internal Vector2 PointToWorld(Vector2 cameraPoint)
		{
			var p = SMPL.scene.MapCoordsToPixel(cameraPoint.ToSFML(), renderTexture.GetView());
			return new(p.X, p.Y);
		}

		private void Init(uint resolutionX, uint resolutionY)
		{
			resolutionX = (uint)((int)resolutionX).Limit(0, (int)Texture.MaximumSize);
			resolutionY = (uint)((int)resolutionY).Limit(0, (int)Texture.MaximumSize);
			renderTexture = new(resolutionX, resolutionY);
			Resolution = new(resolutionX, resolutionY);

			SetPart(new Family());
			SetPart(new Area());
		}
		internal void DrawToWindow(RenderWindow window)
		{
			Display();
			var texSz = renderTexture.Size;
			var viewSz = window.GetView().Size;
			var verts = new Vertex[]
			{
				new(-viewSz * 0.5f, new Vector2f()),
				new(new Vector2f(viewSz.X * 0.5f, -viewSz.Y * 0.5f), new Vector2f(texSz.X, 0)),
				new(viewSz * 0.5f, new Vector2f(texSz.X, texSz.Y)),
				new(new Vector2f(-viewSz.X * 0.5f, viewSz.Y * 0.5f), new Vector2f(0, texSz.Y))
			};

			window.Draw(verts, PrimitiveType.Quads, new(Texture));
		}

		private Area GetArea()
		{
			if (HasPart<Area>() == false)
				SetPart(new Area());

			return GetPart<Area>();
		}
	}
}