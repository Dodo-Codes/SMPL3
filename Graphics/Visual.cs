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
		public int Depth
		{
			get => depth;
			set
			{
				var objs = Scene.CurrentScene.objsDepth;

				TryCreateDepth(depth);

				if(objs[depth].Contains(this))
					objs[depth].Remove(this);

				depth = value;

				TryCreateDepth(depth);
				objs[depth].Add(this);

				void TryCreateDepth(int depth)
				{
					if(objs.ContainsKey(depth) == false)
						objs[depth] = new();
				}
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
