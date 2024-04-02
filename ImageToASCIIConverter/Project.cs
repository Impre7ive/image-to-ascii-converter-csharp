namespace ImageToASCIIConverter
{
	public class Project
	{
		public required string SourcePath { get; set; }
		public required string ResultPath { get; set; }
		public required string Extension { get; set; }
		public required string Folder { get; set; }
		public required Category Category { get; set; }	
	}
}
