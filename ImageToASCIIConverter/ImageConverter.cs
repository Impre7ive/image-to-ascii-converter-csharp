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
			var newHeight = _renderer.GetASCIITextHeight(image);
			var scaledImage = new Bitmap(image, new Size(_renderer.asciiImageTextWidth, newHeight));
			var result = _renderer.GetFrameASCII(scaledImage);
			var originalSizeAsciiFrame = _renderer.GetAsciiFrame(image, scaledImage, result.ToString());

			SaveToTxt(project, result);
			SaveToImage(project, originalSizeAsciiFrame);

			image.Dispose();
			scaledImage.Dispose();
			result.Clear();
			originalSizeAsciiFrame.Dispose();

			Console.WriteLine("Image created successfully!");
		}

		private void SaveToImage(Project project, Bitmap bitmap)
		{
			var format = _renderer.GetFormat(project.Extension) ?? ImageFormat.Bmp;
			bitmap.Save(project.ResultImagePath, format);
		}

		private static void SaveToTxt(Project project, StringBuilder result)
		{
			File.WriteAllText(project.ResultTxtPath, result.ToString());
		}
	}
}