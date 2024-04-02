namespace ImageToASCIIConverter
{
	public static class ProjectManager
	{
		private static readonly List<Project> projects = new List<Project>();

		static ProjectManager()
		{
			//get dirs
			//cycle
			//get file
			//projects.Add(new Project());
		}

		public static void ShowProjectList()
		{
			throw new NotImplementedException();
		}

		public static Project? GetProject(int projectNumber)
		{
			return projectNumber > 0 && projectNumber <= projects.Count ? projects[projectNumber - 1] : null;
		}
	}
}
