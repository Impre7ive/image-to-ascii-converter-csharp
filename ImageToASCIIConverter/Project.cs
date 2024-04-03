namespace ImageToASCIIConverter
{
	public class Project
	{
		public required string SourcePath { get; set; }
		public required string ResultTxtPath { get; set; }
		public required string ResultImagePath { get; set; }
		public required string Extension { get; set; }
		public required string Folder { get; set; }
		public required Category Category { get; set; }	
	}
}
