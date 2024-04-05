using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ImageToASCIIConverter
{
	public class Renderer
	{
		public readonly string asciiContainer = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
		public readonly int asciiImageTextWidth = 200;//170
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

		public ImageFormat? GetFormat(string fileExtension)
		{
			switch (fileExtension.ToLower())
			{
				case ".bmp":
					return ImageFormat.Bmp;
				case ".gif":
					return ImageFormat.Gif;
				case ".ico":
					return ImageFormat.Icon;
				case ".jpeg":
				case ".jpg":
					return ImageFormat.Jpeg;
				case ".png":
					return ImageFormat.Png;
				default:
					return null;
			}
		}

		public Bitmap GetAsciiFrame(Image animation, Bitmap scaledFrame, string asciiCode)
		{
			var font = new Font("Courier New", (float)animation.Width / (float)scaledFrame.Width);
			var bitmap = new Bitmap(animation.Width, animation.Height);

			float x, y;
			x = y = 0;

			int indexX, indexY;
			indexX = indexY = 0;

			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(ColorTranslator.FromHtml("#171717"));

				foreach (char c in asciiCode)
				{
					if (c == '\n')
					{
						indexY++;
						indexX = 0;
						x = 0;
						y += font.Height + ((float)animation.Height / ((float)font.Height * (float)scaledFrame.Height) - 1.12f);
					}
					else if (c != '\r')
					{
						Color charColor = scaledFrame.GetPixel(indexX, indexY);
						graphics.DrawString(c.ToString(), font, new SolidBrush(charColor), new PointF(x, y));
						x += font.SizeInPoints;
						indexX++;
					}
				}
			}

			return bitmap;
		}

		public byte[] ImageToByte(Bitmap img)
		{
			using (var stream = new MemoryStream())
			{
				img.Save(stream, ImageFormat.Jpeg);
				return stream.ToArray();
			}
		}
	}
}