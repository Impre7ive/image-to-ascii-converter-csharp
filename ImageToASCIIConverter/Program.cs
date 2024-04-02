namespace ImageToASCIIConverter
{
	internal class Program
	{
		static void Main()
		{
			if (!Directory.Exists("projects"))
			{
				Console.WriteLine("Project container not found. Creating a new one...");
				Directory.CreateDirectory("projects");
				Console.WriteLine("The project directory \"\u001b[32mprojects\u001b[0m\" created successfully.");

				return;
			}

			ProjectManager.ShowProjectList();

			var projectNumberString = Console.ReadLine();
			int projectNumber;
			int.TryParse(projectNumberString, out projectNumber);
			var project = ProjectManager.GetProject(projectNumber);
			
			if (project != null)
			{
				var converter = FormatFactory.GetConverter(project.Category);
				converter?.Build();
			}
		}
	}
}