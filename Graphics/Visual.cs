namespace SMPL.Graphics
{
	public abstract class Visual : BaseObject
	{
		private int depth;

		public Color Tint { get; set; } = Color.White;
		public BlendMode BlendMode { get; set; } = BlendMode.Alpha;
		public bool IsHidden { get; set; }
		public string TexturePath { get; set; }
		public string ShaderPath { get; set; }
		[JsonIgnore]
		public Texture Texture
			=> TexturePath != null && GetTextures().ContainsKey(TexturePath) ? GetTextures()[TexturePath] : null;
		[JsonIgnore]
		public Shader Shader
			=> ShaderPath != null && GetShaders().ContainsKey(ShaderPath) ? GetShaders()[ShaderPath] : null;
		public Camera DrawTarget { get; set; }
		public int Depth
		{
			get => depth;
			set
			{
				var objs = Scene.CurrentScene.objsDepth;

				if(objs.ContainsKey(depth) == false)
					objs[depth] = new();

				if(objs[depth].Contains(this))
					objs[depth].Remove(this);

				depth = value;
				objs[depth].Add(this);
			}
		}

		public Visual()
		{
			Depth = 0;
		}

		protected abstract void OnDraw();
		internal void Draw() => OnDraw();

		private static Dictionary<string, Texture> GetTextures() => Scene.CurrentScene.Textures;
		private static Dictionary<string, Shader> GetShaders() => Scene.CurrentScene.Shaders;
	}
}
