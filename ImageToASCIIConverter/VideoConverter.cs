using System;
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

			// Create a VideoWriter object to write the modified frames
			using var videoWriter = new VideoWriter(project.ResultVideoPath, FourCC.Default, 30, new OpenCvSharp.Size((int)videoCapture.FrameWidth, (int)videoCapture.FrameHeight));
			Mat frame = new Mat();

			// Loop through the frames
			while (true)
			{
				videoCapture.Read(frame);

				if (frame.Empty())
					break;

				var bitmap = BitmapConverter.ToBitmap(frame);
				var newHeight = _renderer.GetASCIITextHeight(bitmap);
				var scaledFrame = new Bitmap(bitmap, new System.Drawing.Size(_renderer.asciiImageTextWidth, newHeight));
				StringBuilder resultAscii = _renderer.GetFrameASCII(scaledFrame);
				var originalSizeAsciiFrame = _renderer.GetAsciiFrame(bitmap, scaledFrame, resultAscii.ToString());
				var newFrame = BitmapConverter.ToMat(originalSizeAsciiFrame);

				videoWriter.Write(newFrame);

				// Display the frame (optional)
				Cv2.ImShow("Frame", newFrame);
				Cv2.WaitKey(1); // Adjust waitKey value to control frame rate
			}

			videoCapture.Release();
			videoWriter.Release();
			Cv2.DestroyAllWindows();

			ExtractAudio(project.SourcePath, project.SourcePath.Replace(".mp4", "-gg.mp3"));
			MergeAudioAndVideo(project.ResultVideoPath, project.SourcePath.Replace(".mp4", "-gg.mp3"), project.ResultVideoPath.Replace(".mp4", "-music.mp4"));
		}
		static void ExtractAudio(string inputVideoPath, string audioPath)
		{
			string ffmpegArgs = $"-i {inputVideoPath} -q:a 0 -map a {audioPath}";

			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = ffmpegArgs,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = true,
				CreateNoWindow = false
			};

			using (Process process = Process.Start(startInfo))
			{
				process.WaitForExit();
			}
		}

		static void MergeAudioAndVideo(string inputVideoPath, string audioPath, string outputVideoPath)
		{
			string ffmpegArgs = $"-i {inputVideoPath} -i {audioPath} -c:v copy -c:a aac -strict experimental {outputVideoPath}";

			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = ffmpegArgs,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = true,
				CreateNoWindow = false
			};

			using (Process process = Process.Start(startInfo))
			{
				process.WaitForExit();
			}
		}

	}
}
