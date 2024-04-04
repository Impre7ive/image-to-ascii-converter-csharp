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
			SaveToImage(result.ToString(), project, scaledImage);
		}
		private static void SaveToTxt(Project project, StringBuilder result)
		{
			File.WriteAllText(project.ResultTxtPath, result.ToString());
		}

		private void SaveToImage(string asciiCode, Project project, Bitmap resizedImage)
		{
			Font font = new Font("Courier New", 12);
			Bitmap bitmap = new Bitmap((int)(resizedImage.Width * 12.1595), (int)(resizedImage.Height * 18.45));

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