namespace ImageToASCIIConverter
{
	internal class Program
	{
		static void Main()
		{
			ProjectManager.ShowProjectList();
			int projectNumber = ProjectManager.SetProjectNumber();
			var project = ProjectManager.GetProject(projectNumber);
			
			if (project != null)
			{
				var converter = FormatFactory.GetConverter(project.Category);

				if (converter != null)
				{
					var renderContext = new Renderer(converter);
					renderContext.Convert(project);
				}
			}
		}
	}
}