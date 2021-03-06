global using System.Collections.ObjectModel;
global using System.Numerics;
global using System.Reflection;
global using Fasterflect;
global using Newtonsoft.Json;
global using SFML.Audio;
global using SFML.Graphics;
global using SFML.System;
global using SFML.Window;
global using SMPL.Commands;
global using SMPL.Graphics;
global using SMPL.Tools;
global using Sprite = SMPL.Graphics.Sprite;
global using Color = System.Drawing.Color;

namespace SMPL
{
	public partial class SMPL : Form
	{
		internal static SMPL instance;
		internal static RenderWindow scene, game;

		private bool isSelecting;
		private Vector2 prevMousePos, selectStartPos, rightClickPos;
		private readonly System.Windows.Forms.Timer loop;
		private readonly Camera sceneCamera;

		public SMPL()
		{
			InitializeComponent();
			instance = this;

			Scene.Init(new(), null);
			var sz = new Vector2(1920, 1080);//Settings.ScreenResolution;
			Scene.MainCamera = new(sz);
			sceneCamera = new(sz);

			scene = new(sceneTab.Handle);
			game = new(gameTab.Handle);

			loop = new() { Interval = 1 };
			loop.Tick += OnUpdate;
			loop.Start();

			CenterView(scene);
			CenterView(game);
			UpdateZoom();

			var spr = new Sprite() { Position = new(100, 200) };
			var v = Dodo.DoGet(spr, "GetPositionFromSelf", new Vector2(0, 0));
			var v2 = Dodo.DoGet(spr, "GetPositionFromSelf", new Vector2(100, 0));
			Dodo.DoGet(spr, "g", 3);

			void CenterView(RenderWindow window)
			{
				var view = window.GetView();
				view.Center = new();
				window.SetView(view);
			}
		}
		public static void StopGame()
		{
			Scene.CurrentScene?.GameStop();
			game.Close();
		}

		private void OnUpdate(object sender, EventArgs e)
		{
			Tools.Time.Update();

			if(topLeftTabs.SelectedIndex == 0)
				ProcessCamera(sceneCamera, scene);
			else
				ProcessCamera(Scene.MainCamera, game);

			if(log.SelectionStart == 0)
				log.SelectionStart = 1;

			void ProcessCamera(Camera camera, RenderWindow window)
			{
				window.Size = new((uint)topLeftTabs.Width, (uint)topLeftTabs.Height);

				window.Clear();
				camera.Fill();
				camera.Update();

				if(camera == sceneCamera)
				{
					var view = camera.RenderTexture.GetView();
					var sc = camera.Scale;
					view.Size = new(window.Size.X * sc, window.Size.Y * sc);
					camera.RenderTexture.SetView(view);

					TryDrawGrid();
					TryShowMousePos();

					var prevMainCam = Scene.MainCamera;
					Scene.MainCamera = camera;
					Scene.CurrentScene.DrawObjs();
					Scene.MainCamera = prevMainCam;

					TryDrawSelection();
				}
				else
				{
					Scene.UpdateCurrentScene();
					Scene.CurrentScene.UpdateObjs();
					Scene.CurrentScene.DrawObjs();
				}

				camera.DrawToWindow(window);
				window.Display();
			}
		}
		private void TryDrawGrid()
		{
			if(gridThickness.Value == gridThickness.Minimum)
				return;

			var cellVerts = new VertexArray(PrimitiveType.Quads);
			var specialCellVerts = new VertexArray(PrimitiveType.Quads);
			var sc = sceneCamera.Scale;
			var sz = new Vector2(sceneTab.Width, sceneTab.Height) * sc;
			var thickness = (float)gridThickness.Value;
			var spacing = GetGridSpacing();

			thickness *= sc;
			thickness *= 0.5f;

			for(float i = 0; i <= sz.X * 4; i += spacing)
			{
				var x = sceneCamera.Position.X - sz.X * 2 + i;
				var y = sceneCamera.Position.Y;
				var top = new Vector2(x, y - sz.Y * 2).PointToGrid(new(spacing));
				var bot = new Vector2(x, y + sz.Y * 2).PointToGrid(new(spacing));
				var col = GetColor(top.X);
				var verts = GetVertexArray(top.X);

				verts.Append(new(top.PointMoveAtAngle(180, thickness, false).ToSFML(), col));
				verts.Append(new(top.PointMoveAtAngle(0, thickness, false).ToSFML(), col));
				verts.Append(new(bot.PointMoveAtAngle(0, thickness, false).ToSFML(), col));
				verts.Append(new(bot.PointMoveAtAngle(180, thickness, false).ToSFML(), col));
			}
			for(float i = 0; i <= sz.Y * 4; i += spacing)
			{
				var x = sceneCamera.Position.X;
				var y = sceneCamera.Position.Y - sz.Y * 2 + i;
				var left = new Vector2(x - sz.X * 2, y).PointToGrid(new(spacing));
				var right = new Vector2(x + sz.X * 2, y).PointToGrid(new(spacing));
				var col = GetColor(left.Y);
				var verts = GetVertexArray(left.Y);

				verts.Append(new(left.PointMoveAtAngle(270, thickness, false).ToSFML(), col));
				verts.Append(new(left.PointMoveAtAngle(90, thickness, false).ToSFML(), col));
				verts.Append(new(right.PointMoveAtAngle(90, thickness, false).ToSFML(), col));
				verts.Append(new(right.PointMoveAtAngle(270, thickness, false).ToSFML(), col));
			}

			sceneCamera.RenderTexture.Draw(cellVerts);
			sceneCamera.RenderTexture.Draw(specialCellVerts);

			SFML.Graphics.Color GetColor(float coordinate)
			{
				if(coordinate == 0)
					return SFML.Graphics.Color.Yellow;
				else if(coordinate % 1000 == 0)
					return SFML.Graphics.Color.White;

				return new SFML.Graphics.Color(50, 50, 50);
			}
			VertexArray GetVertexArray(float coordinate)
			{
				return coordinate == 0 || coordinate % 1000 == 0 ? specialCellVerts : cellVerts;
			}
		}
		private void TryShowMousePos()
		{
			if(sceneMousePos.Visible == false)
				return;

			var gridSpacing = GetGridSpacing();
			var mousePos = sceneCamera.MouseCursorPosition;
			var inGrid = mousePos.PointToGrid(new(gridSpacing)) + new Vector2(gridSpacing) * 0.5f;
			sceneMousePos.Text =
				$"Cursor [{(int)mousePos.X} {(int)mousePos.Y}]\n" +
				$"Grid [{(int)inGrid.X} {(int)inGrid.Y}]";
		}
		private void TryDrawSelection()
		{
			if(isSelecting == false)
				return;

			var mousePos = Mouse.GetPosition(scene);
			var topLeft = selectStartPos;
			var botRight = new Vector2(mousePos.X, mousePos.Y);
			var topRight = new Vector2(botRight.X, topLeft.Y);
			var botLeft = new Vector2(topLeft.X, botRight.Y);
			var tl = sceneCamera.PointToCamera(topLeft).ToSFML();
			var tr = sceneCamera.PointToCamera(topRight).ToSFML();
			var br = sceneCamera.PointToCamera(botRight).ToSFML();
			var bl = sceneCamera.PointToCamera(botLeft).ToSFML();
			var fillCol = new SFML.Graphics.Color(0, 255, 255, 100);
			var outCol = Color.Black;
			var fill = new Vertex[] { new(tl, fillCol), new(tr, fillCol), new(br, fillCol), new(bl, fillCol), };

			new Line(tl.ToSystem(), tr.ToSystem()).Draw(sceneCamera, outCol);
			new Line(tr.ToSystem(), br.ToSystem()).Draw(sceneCamera, outCol);
			new Line(br.ToSystem(), bl.ToSystem()).Draw(sceneCamera, outCol);
			new Line(bl.ToSystem(), tl.ToSystem()).Draw(sceneCamera, outCol);

			sceneCamera.RenderTexture.Draw(fill, PrimitiveType.Quads);
		}
		private float GetGridSpacing()
		{
			return MathF.Max(gridSpacing.Text.ToNumber(), 10);
		}
		#region Scene
		private void OnMouseLeaveScene(object sender, EventArgs e)
		{
			sceneMousePos.Hide();
			SceneSelect();
		}
		private void OnMouseEnterScene(object sender, EventArgs e)
		{
			sceneMousePos.Show();
		}
		private void OnMouseMoveScene(object sender, MouseEventArgs e)
		{
			var sc = sceneCamera.Scale;
			var scale = sceneCamera.Size / new Vector2(topLeftTabs.Width, topLeftTabs.Height);
			var pos = new Vector2(MousePosition.X, MousePosition.Y) * scale;
			var dist = prevMousePos.DistanceBetweenPoints(pos) * sc;
			var ang = prevMousePos.AngleBetweenPoints(pos);
			prevMousePos = pos;

			if(e.Button != MouseButtons.Middle || dist == 0)
				return;

			sceneCamera.Position = sceneCamera.Position.PointMoveAtAngle(sceneCamera.Angle + ang, -dist, false);

			System.Windows.Forms.Cursor.Current = Cursors.NoMove2D;
		}
		private void OnSceneZoom(object sender, EventArgs e)
		{
			UpdateZoom();
		}
		private void OnSceneRotate(object sender, EventArgs e)
		{
			sceneCamera.Angle = ((float)sceneAngle.Value).Map(0, 100, 0, 360);
		}
		private void OnMouseDownScene(object sender, MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
				return;

			sceneTab.Focus();

			isSelecting = true;
			var pos = Mouse.GetPosition(scene);
			selectStartPos = new(pos.X, pos.Y);
		}
		private void OnMouseUpScene(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
				rightClickPos = sceneCamera.MouseCursorPosition;

			if(e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
				return;

			isSelecting = false;
			SceneSelect();
		}
		private void OnGridSpacingChange(object sender, EventArgs e)
		{
			if(gridSpacing.Text == "" || gridSpacing.Text.ToNumber() < 10)
				gridSpacing.Text = "10";
		}
		private void OnKeyDownTopLeftTabs(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Hotkey.TryTriggerHotkeys();
		}
		private void OnSceneStatusClick(object sender, EventArgs e)
		{
			sceneTab.Focus();
		}

		#region RightClick
		private void OnSceneRightClickMenuResetView(object sender, EventArgs e)
		{
			sceneAngle.Value = 0;
			sceneZoom.Value = 10;
			sceneCamera.Position = new();
			sceneCamera.Angle = 0;
			sceneCamera.Scale = 1;
			UpdateZoom();
		}
		private void OnSceneRightClickMenuCreateSprite(object sender, EventArgs e)
		{
			new Sprite { Position = rightClickPos };
		}
		#endregion

		private void UpdateZoom()
		{
			sceneCamera.Scale = ((float)sceneZoom.Value).Map(0, 100, 0.3f, 10f);
		}
		private void SceneSelect()
		{
			var ang = sceneCamera.Angle;
			var topLeft = selectStartPos;
			var botRight = sceneCamera.MouseCursorPosition;
			var side = topLeft.DistanceBetweenPoints(botRight) / 1.4f;
			var topRight = topLeft.PointMoveAtAngle(ang, side, false);
			var botLeft = topLeft.PointMoveAtAngle(ang + 90, side, false);
		}
		#endregion
		#region Log
		private void OnOutputTextChange(object sender, EventArgs e)
		{
			if(log.Text.Length > 0 && log.Text[0] != '\n')
				log.Text = $"\n{log.Text}";
		}
		private void OnOutputKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(log.SelectionStart == 1 && log.SelectionLength == 0 && e.KeyCode == Keys.Back)
			{
				e.Handled = true;
				return;
			}

			if(e.Control == false || e.KeyCode != Keys.V)
				return;

			((RichTextBox)sender).Paste(DataFormats.GetFormat("Text"));
			e.Handled = true;
		}
		private void OnCommandKeyPress(object sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				Command.TryExecute(command.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
				command.Text = "";
			}
		}

		internal void ClearLog()
		{
			log.Clear();
			log.Text = "\n";
		}
		internal void Log(object message, Color color, bool newLine)
		{
			if(outputActive.Checked)
				return;

			if(color == default)
				color = Color.White;

			var newLineStr = newLine ? "\n" : "";
			log.SelectionStart = log.TextLength;
			log.SelectionLength = 0;
			log.SelectionColor = color;
			log.AppendText($"{message}{newLineStr}");
			log.SelectionColor = Color.White;

			log.ScrollToCaret();
		}
		#endregion
	}
}