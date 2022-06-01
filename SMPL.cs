global using SFML.Graphics;
global using SFML.Audio;
global using SFML.System;
global using SFML.Window;
global using Color = System.Drawing.Color;
global using SMPL.Parts;
global using SMPL.Graphics;
global using System.Numerics;
global using SMPL.Tools;
global using Newtonsoft.Json;

namespace SMPL
{
	public partial class SMPL : Form
	{
		internal static SMPL instance;
		internal static RenderWindow scene, game;

		private Vector2 prevMousePos;
		private readonly System.Windows.Forms.Timer loop;
		private readonly Camera sceneCamera;

		public SMPL()
		{
			InitializeComponent();
			instance = this;

			//Scene.Init(startingScene, loadingScene);
			var sz = new Vector2(1920, 1080);//Settings.ScreenResolution;
			Scene.MainCamera = new(sz);
			sceneCamera = new(sz);

			scene = new(sceneTab.Handle);
			game = new(gameTab.Handle);

			loop = new() { Interval = 1 };
			loop.Tick += OnUpdate;
			loop.Start();

			Debug.LogError("f");

			CenterView(scene);
			CenterView(game);
			UpdateZoom();

			void CenterView(RenderWindow window)
			{
				var view = window.GetView();
				view.Center = new();
				window.SetView(view);
			}
		}

		private void OnUpdate(object sender, EventArgs e)
		{
			Tools.Time.Update();

			if (topLeftTabs.SelectedIndex == 0)
				ProcessCamera(sceneCamera, scene);
			else
				ProcessCamera(Scene.MainCamera, game);

			if (log.SelectionStart == 0)
				log.SelectionStart = 1;

			void ProcessCamera(Camera camera, RenderWindow window)
			{
				var view = camera.RenderTexture.GetView();
				view.Size = new(topLeftTabs.Size.Width, topLeftTabs.Size.Height);
				camera.RenderTexture.SetView(view);

				window.Size = new((uint)topLeftTabs.Size.Width, (uint)topLeftTabs.Size.Height);
				window.Clear();
				camera.Fill();
				camera.Update();

				if (camera == Scene.MainCamera)
					Scene.UpdateCurrentScene();
				else
				{
					TryDrawGrid();
					TryShowMousePos();
				}

				camera.DrawToWindow(window);

				window.Display();
			}
		}
		private void TryDrawGrid()
		{
			if (gridThickness.Value == gridThickness.Minimum)
				return;

			var verts = new VertexArray(PrimitiveType.Quads);
			var area = sceneCamera.GetPart<Area>();
			var sc = area.Scale;
			var sz = new Vector2(sceneTab.Width, sceneTab.Height) * sc;
			var thickness = (float)gridThickness.Value;
			var spacing = GetGridSpacing();

			thickness *= sc;

			for (float i = 0; i <= sz.X * 4; i += spacing)
			{
				var x = area.Position.X - sz.X * 2 + i;
				var y = area.Position.Y;
				var top = new Vector2(x, y - sz.Y * 2).PointToGrid(new(spacing));
				var bot = new Vector2(x, y + sz.Y * 2).PointToGrid(new(spacing));
				var col = GetColor(top.X);

				verts.Append(new(top.ToSFML(), col));
				verts.Append(new(top.PointMoveAtAngle(0, thickness, false).ToSFML(), col));
				verts.Append(new(bot.PointMoveAtAngle(0, thickness, false).ToSFML(), col));
				verts.Append(new(bot.ToSFML(), col));
			}
			for (float i = 0; i <= sz.Y * 4; i += spacing)
			{
				var x = area.Position.X;
				var y = area.Position.Y - sz.Y * 2 + i;
				var left = new Vector2(x - sz.X * 2, y).PointToGrid(new(spacing));
				var right = new Vector2(x + sz.X * 2, y).PointToGrid(new(spacing));
				var col = GetColor(left.Y);

				verts.Append(new(left.ToSFML(), col));
				verts.Append(new(left.PointMoveAtAngle(90, thickness, false).ToSFML(), col));
				verts.Append(new(right.PointMoveAtAngle(90, thickness, false).ToSFML(), col));
				verts.Append(new(right.ToSFML(), col));
			}

			sceneCamera.RenderTexture.Draw(verts);

			SFML.Graphics.Color GetColor(float coordinate)
			{
				if (coordinate == 0)
					return SFML.Graphics.Color.Yellow;
				else if (coordinate % 1000 == 0)
					return SFML.Graphics.Color.White;

				return new SFML.Graphics.Color(255, 255, 255, 100);
			}
		}
		private void TryShowMousePos()
		{
			if (sceneMousePos.Visible == false)
				return;

			var gridSpacing = GetGridSpacing();
			var mousePos = sceneCamera.MouseCursorPosition;
			var inGrid = mousePos.PointToGrid(new(gridSpacing)) + new Vector2(gridSpacing) * 0.5f;
			sceneMousePos.Text =
				$"Cursor [{(int)mousePos.X} {(int)mousePos.Y}]\n" +
				$"Grid [{(int)inGrid.X} {(int)inGrid.Y}]";
		}
		private float GetGridSpacing()
		{
			return MathF.Max(gridSpacing.Text.ToNumber(), 10);
		}
		#region Scene
		private void OnMouseLeaveScene(object sender, EventArgs e)
		{
			sceneMousePos.Hide();
		}
		private void OnMouseEnterScene(object sender, EventArgs e)
		{
			sceneMousePos.Show();
		}
		private void OnMouseMoveScene(object sender, MouseEventArgs e)
		{
			var sc = sceneCamera.GetPart<Area>().Scale;
			var scale = sceneCamera.Size * sc / new Vector2(topLeftTabs.Width, topLeftTabs.Height);
			var pos = new Vector2(MousePosition.X, MousePosition.Y) * scale;
			var dist = prevMousePos.DistanceBetweenPoints(pos) * sc;
			var ang = prevMousePos.AngleBetweenPoints(pos);
			prevMousePos = pos;
			
			if (e.Button != MouseButtons.Middle || dist == 0)
				return;

			var area = sceneCamera.GetPart<Area>();
			area.Position = area.Position.PointMoveAtAngle(sceneCamera.GetPart<Area>().Angle + ang, -dist, false);

			System.Windows.Forms.Cursor.Current = Cursors.NoMove2D;
		}
		private void OnSceneZoom(object sender, EventArgs e)
		{
			UpdateZoom();
		}
		private void OnSceneRotate(object sender, EventArgs e)
		{
			sceneCamera.GetPart<Area>().Angle = ((float)sceneAngle.Value).Map(0, 100, 0, 360);
		}
		private void OnResetSceneCamera(object sender, EventArgs e)
		{
			sceneCamera.SetPart(new Area());
		}

		private void UpdateZoom()
		{
			sceneCamera.GetPart<Area>().Scale = ((float)sceneZoom.Value).Map(0, 100, 0.3f, 10f);
		}
		#endregion
		#region Output
		private void OnOutputClear(object sender, EventArgs e)
		{
			log.Clear();
			log.Text = "\n";
		}
		private void OnOutputTextChange(object sender, EventArgs e)
		{
			if (log.Text.Length > 0 && log.Text[0] != '\n')
				log.Text = $"\n{log.Text}";
		}
		private void OnOutputKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (log.SelectionStart == 1 && log.SelectionLength == 0 && e.KeyCode == Keys.Back)
			{
				e.Handled = true;
				return;
			}

			if (e.Control == false || e.KeyCode != Keys.V)
				return;

			((RichTextBox)sender).Paste(DataFormats.GetFormat("Text"));
			e.Handled = true;
		}

		internal void LogToOutput(object message, Color color, bool newLine)
		{
			if (outputActive.Checked)
				return;

			if (color == default)
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