namespace ImageToASCIIConverter
{
	public class VideoConverter : IConverter
	{
		private readonly Renderer _renderer;
		public VideoConverter(Renderer renderer)
		{
			_renderer = renderer;
		}

		public void Convert(Project project)
		{
			throw new NotImplementedException();
		}
	}
}
