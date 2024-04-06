namespace ImageToASCIIConverter
{
	public class Project
	{
		public required string SourcePath { get; set; }
		public string ResultTxtPath { get; set; } = string.Empty;
		public string ResultImagePath { get; set; } = string.Empty;
		public string ResultAnimationPath { get; set; } = string.Empty;
		public string ResultVideoPath { get; set; } = string.Empty;
		public string ResultAudioPath { get; set; } = string.Empty;
		public string ResultVideoWithSoundPath { get; set; } = string.Empty;
		public required string Extension { get; set; }
		public required string Folder { get; set; }
		public required Category Category { get; set; }	
	}
}
