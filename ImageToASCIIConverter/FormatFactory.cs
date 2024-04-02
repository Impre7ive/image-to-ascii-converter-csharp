namespace ImageToASCIIConverter
{
	static class FormatFactory
	{
		private static Dictionary<Category, IConverter> _strategies = new Dictionary<Category, IConverter>() 
		{
			{ Category.Image, new ImageStrategy() }
		};
		public static IConverter? GetConverter(Category category)
		{
			return _strategies.ContainsKey(category) ? _strategies[category] : null;
		}
	}
}
