using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ImageToASCIIConverter
{
	public class ImageConverter : IConverter
	{
		private readonly Renderer _renderer;
		public ImageConverter(Renderer renderer) 
		{
			_renderer = renderer;
		}
		public void Convert(Project project)
		{
			var image = new Bitmap(project.SourcePath);
			int newHeight = _renderer.GetASCIITextHeight(image);
			var scaledImage = new Bitmap(image, new Size(_renderer.asciiImageTextWidth, newHeight));
			StringBuilder result = _renderer.GetFrameASCII(scaledImage);
			SaveToTxt(project, result);
			SaveToImage(result.ToString(), project, scaledImage, image);
		}
		private static void SaveToTxt(Project project, StringBuilder result)
		{
			File.WriteAllText(project.ResultTxtPath, result.ToString());
		}

		private void SaveToImage(string asciiCode, Project project, Bitmap resizedImage, Bitmap originalImage)
		{
			Font font = new ("Courier New", (float)originalImage.Width / (float)resizedImage.Width);
			Bitmap bitmap = new Bitmap(originalImage.Width, originalImage.Height);
			float x = 0;
			float y = 0;

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
						y += font.Height + ((float)originalImage.Height / ((float)font.Height * (float)resizedImage.Height) - 1.12f) ;
					}
					else if (c != '\r')
					{
						Color charColor = resizedImage.GetPixel(indexX, indexY);
						graphics.DrawString(c.ToString(), font, new SolidBrush(charColor), new PointF(x, y));
						x += font.SizeInPoints;
						indexX++;
					}
				}
			}

			bitmap.Save(project.ResultImagePath, ImageFormat.Bmp);
		}
	}
}