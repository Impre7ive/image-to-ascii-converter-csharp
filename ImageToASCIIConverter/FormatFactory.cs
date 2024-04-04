namespace ImageToASCIIConverter
{
	static class FormatFactory
	{
		private static Dictionary<Category, Func<Renderer, IConverter>> _strategies = new Dictionary<Category, Func<Renderer, IConverter>>() 
		{
			{ Category.Image, renderer => new ImageConverter(renderer) }
		};
		public static IConverter? GetConverter(Category category, Renderer renderer)
		{
			return _strategies.ContainsKey(category) ? _strategies[category](renderer) : null;
		}
	}
}
