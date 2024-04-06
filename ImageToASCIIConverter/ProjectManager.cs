namespace ImageToASCIIConverter
{
	public static class ProjectManager
	{
		private static readonly List<Project> _projects = new();
		private static readonly Dictionary<Category, Action<Project, string, string>> _projectPath = new() {
			{Category.Image, (project, dir, extension) => { project.ResultTxtPath = dir + $"\\result.txt"; project.ResultImagePath = dir + $"\\result{extension}";} },
			{Category.Animation, (project, dir, extension) => { project.ResultAnimationPath = dir + $"\\result{extension}";} },
			{Category.Video, (project, dir, extension) => { 
				project.ResultVideoPath = dir + $"\\result{extension}";
				project.ResultAudioPath = dir + $"\\result-audio.mp3";
				project.ResultVideoWithSoundPath = dir + $"\\result-with-sound{extension}";
			} },
			{Category.None, (project, dir, extension) => { } }
		};

		private static readonly string[] _imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".ico" };
		private static readonly string[] _videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".mkv" };
		private static readonly string[] _animationExtensions = { ".gif" };

		static ProjectManager()
		{
			CheckIfProjectContainerExists();
			GetProjects();
		}

		private static void GetProjects()
		{
			var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/projects");

			foreach (var dir in dirs)
			{
				var files = Directory.GetFiles(dir, "source.*");
				
				foreach (var file in files) 
				{
					var extension = Path.GetExtension(file);
					var folderName = Path.GetFileName(dir);
					var category = GetCategory(extension);

					var project = new Project
					{
						Category = category,
						Extension = extension,
						Folder = folderName,
						SourcePath = file,
					};

					_projectPath[category](project, dir, extension);
					_projects.Add(project);
				}
			}
		}

		private static Category GetCategory(string extension)
		{
			Category result = Category.None;

			if (_imageExtensions.Contains(extension))
			{
				result = Category.Image;
			}
			else if (_animationExtensions.Contains(extension))
			{
				result = Category.Animation;
			}
			else if (_videoExtensions.Contains(extension))
			{
				result = Category.Video;
			}

			return result;
		}

		private static void CheckIfProjectContainerExists()
		{
			if (!Directory.Exists("projects"))
			{
				Console.WriteLine("Project container not found. Creating a new one...");
				Directory.CreateDirectory("projects");
				Console.WriteLine("The project directory \"\u001b[32mprojects\u001b[0m\" created successfully.");
			}
		}

		public static void ShowProjectList()
		{
			Console.WriteLine("Choose a project:");
			
			for (int i = 0; i < _projects.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {_projects[i].Folder}.");
			}
		}

		public static Project? GetProject(int projectNumber)
		{
			return projectNumber > 0 && projectNumber <= _projects.Count ? _projects[projectNumber - 1] : null;
		}

		internal static int SetProjectNumber()
		{
			Console.WriteLine("Your choice: ");
			var projectNumberString = Console.ReadLine();
			int.TryParse(projectNumberString, out int projectNumber);
			return projectNumber;
		}
	}
}
