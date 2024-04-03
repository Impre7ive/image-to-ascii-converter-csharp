using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

			File.WriteAllText(project.ResultTxtPath , result.ToString());
			SaveToImage(result.ToString(), project, resizedImage);

			//_converter.Save();
		}

		private void SaveToImage(string asciiCode, Project project, Bitmap resizedImage)
		{
			Font font = new Font("Courier New", 12);
			Bitmap bitmap = new Bitmap((int)(resizedImage.Width * 12.058), (int)(resizedImage.Height * 18.3));

			float x = 10;
			float y = 10;

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

						x = 10; 
						y += font.Height - 0.85f;
					}
					else if (c != '\r')
					{
						Color charColor = resizedImage.GetPixel(indexX, indexY);
						graphics.DrawString(c.ToString(), font, new SolidBrush(charColor), new PointF(x, y));
						x += font.Size;
						indexX++;
					}
				}
			}

			bitmap.Save(project.ResultImagePath, ImageFormat.Bmp);
		}
	}
}