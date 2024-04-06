using ImageMagick;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageToASCIIConverter
{
	public class AnimationConverter : IConverter
	{
		private readonly Renderer _renderer;
		public AnimationConverter(Renderer renderer)
		{
			_renderer = renderer;
		}

		public void Convert(Project project)
		{	
			var result = Image.FromFile(project.SourcePath);
			SaveToAnimation(project, result);
		}

		private void SaveToAnimation(Project project, Image animation)
		{
			var bitmaps = new List<Bitmap>();

			using (var gifImage = Image.FromFile(project.SourcePath))
			{
				var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]); //FrameDimension.Time
				var frameCount = gifImage.GetFrameCount(dimension);

				for (int i = 0; i < frameCount; i++)
				{
					gifImage.SelectActiveFrame(dimension, i);

					var frame = new Bitmap(gifImage);
					var newHeight = _renderer.GetASCIITextHeight(frame);
					var scaledFrame = new Bitmap(frame, new Size(_renderer.asciiImageTextWidth, newHeight));
					var resultAscii = _renderer.GetFrameASCII(scaledFrame);
					var originalSizeAsciiFrame = _renderer.GetAsciiFrame(animation, scaledFrame, resultAscii.ToString());

					bitmaps.Add(originalSizeAsciiFrame);

					frame.Dispose();
					scaledFrame.Dispose();
					resultAscii.Clear();
				}
			}

			SaveAsGif(bitmaps, project.ResultAnimationPath);
		}

		private void SaveAsGif(List<Bitmap> bitmaps, string resultAnimationPath)
		{
			using (MagickImageCollection collection = new MagickImageCollection())
			{
				foreach (var imagePath in bitmaps)
				{
					var image = new MagickImage(_renderer.ImageToByte(imagePath));
					imagePath.Dispose();
					//image.AnimationDelay = 50;
					collection.Add(image);
				}

				bitmaps.Clear();
				collection.OptimizePlus();
				collection.Write(resultAnimationPath, MagickFormat.Gif);
			}

			Console.WriteLine("GIF created successfully!");
		}	
	}
}
