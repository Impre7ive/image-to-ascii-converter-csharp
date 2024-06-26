﻿namespace ImageToASCIIConverter
{
	public class Program
	{
		static void Main()
		{
			ProjectManager.ShowProjectList();
			var projectNumber = ProjectManager.SetProjectNumber();
			var project = ProjectManager.GetProject(projectNumber);
			
			if (project != null)
			{
				var renderer = new Renderer();
				var converter = FormatFactory.GetConverter(project.Category, renderer);
				converter?.Convert(project);
			}
		}
	}
}