namespace ImageToASCIIConverter
{
	static class FormatFactory
	{
		private static Dictionary<Category, IConverter> strategies = new Dictionary<Category, IConverter>() 
		{
			{ Category.Image, new ImageStrategy() }
		};
		public static IConverter? GetConverter(Category category)
		{
			return strategies.ContainsKey(category) ? strategies[category] : null;
		}
	}
}
