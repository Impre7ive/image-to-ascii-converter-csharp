using System.Drawing;
using System.Text;

namespace ImageToASCIIConverter
{
	public class Renderer
	{
		private readonly string _asciiContainer = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
		private readonly IConverter _converter;
		public Renderer(IConverter converter)
		{
			_converter = converter;
		}

		internal void Convert(Project project)
		{
			var image = new Bitmap(project.SourcePath);
			var newWidth = 460;
			var spacing = 0.5;
			var newHeight = (int)((double)image.Height * newWidth / image.Width * spacing);
			var resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
			var result = new StringBuilder();

			for (int i = 0;  i < resizedImage.Height; i++)
			{
				for (int j = 0; j < resizedImage.Width; j++)
				{
					var pixel = resizedImage.GetPixel(j, i);
					var intensity = 0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B;
					var normalizedIntensity = intensity / 255.0;
					var index = (int)(normalizedIntensity * (_asciiContainer.Length - 1));
					result.Append(_asciiContainer[index]);
				}

				result.AppendLine();
			}

			File.WriteAllText(project.ResultPath , result.ToString());

			//_converter.Save();
		}
	}
}