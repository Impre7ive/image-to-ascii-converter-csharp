using System.Drawing;
using System.Text;

namespace ImageToASCIIConverter
{
	public class Renderer
	{
		public readonly string asciiContainer = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
		public readonly int asciiImageTextWidth = 150;
		public readonly float spacingScale = 0.65f;

		public StringBuilder GetFrameASCII(Bitmap scaledImage)
		{
			var result = new StringBuilder();

			for (int i = 0; i < scaledImage.Height; i++)
			{
				for (int j = 0; j < scaledImage.Width; j++)
				{
					var pixel = scaledImage.GetPixel(j, i);
					var intensity = 0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B;
					var normalizedIntensity = intensity / 255.0;
					var index = (int)(normalizedIntensity * (asciiContainer.Length - 1));
					result.Append(asciiContainer[index]);
				}

				result.AppendLine();
			}

			return result;
		}

		public int GetASCIITextHeight(Bitmap image)
		{
			return (int)((double)image.Height * asciiImageTextWidth / image.Width * spacingScale);
		}
	}
}