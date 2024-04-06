using System.Diagnostics;
using System.Drawing;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImageToASCIIConverter
{
	public class VideoConverter : IConverter
	{
		private readonly Renderer _renderer;
		public VideoConverter(Renderer renderer)
		{
			_renderer = renderer;
		}

		public void Convert(Project project)
		{
			using var videoCapture = new VideoCapture(project.SourcePath);

			if (!videoCapture.IsOpened())
			{
				Console.WriteLine("Failed to open video file.");
				return;
			}

			using var videoWriter = new VideoWriter(project.ResultVideoPath, FourCC.Default, 30, new OpenCvSharp.Size((int)videoCapture.FrameWidth, (int)videoCapture.FrameHeight));
			var frame = new Mat();

			while (true)
			{
				videoCapture.Read(frame);

				if (frame.Empty())
					break;

				var bitmap = BitmapConverter.ToBitmap(frame);
				var newHeight = _renderer.GetASCIITextHeight(bitmap);
				var scaledFrame = new Bitmap(bitmap, new System.Drawing.Size(_renderer.asciiImageTextWidth, newHeight));
				var resultAscii = _renderer.GetFrameASCII(scaledFrame);
				var originalSizeAsciiFrame = _renderer.GetAsciiFrame(bitmap, scaledFrame, resultAscii.ToString());
				var newFrame = BitmapConverter.ToMat(originalSizeAsciiFrame);

				videoWriter.Write(newFrame);

				// Display the frame 
				Cv2.ImShow("Frame", newFrame);
				Cv2.WaitKey(1);
			}

			videoCapture.Release();
			videoWriter.Release();
			Cv2.DestroyAllWindows();

			ExtractAudio(project.SourcePath, project.ResultAudioPath);
			MergeAudioAndVideo(project.ResultVideoPath, project.ResultAudioPath, project.ResultVideoWithSoundPath);

			Console.WriteLine("Video created successfully!");
		}
		static void ExtractAudio(string inputVideoPath, string audioPath)
		{
			var ffmpegArgs = $"-i {inputVideoPath} -q:a 0 -map a {audioPath}";

			var startInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = ffmpegArgs,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = true,
				CreateNoWindow = false
			};

			using (var process = Process.Start(startInfo)!)
			{
				process.WaitForExit();
			}
		}

		static void MergeAudioAndVideo(string inputVideoPath, string audioPath, string outputVideoPath)
		{
			var ffmpegArgs = $"-i {inputVideoPath} -i {audioPath} -c:v copy -c:a aac -strict experimental {outputVideoPath}";

			var startInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = ffmpegArgs,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = true,
				CreateNoWindow = false
			};

			using (var process = Process.Start(startInfo)!)
			{
				process.WaitForExit();
			}
		}

	}
}
