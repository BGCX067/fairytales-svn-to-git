using System;
using System.Collections.Generic;
using System.IO;

namespace ConeFabric.FairyTales.Core
{
    public class ProjectModel
    {
        public string PersistanceDirectory { set { persistanceDirectory = value; } }

        public void AddProject(string name)
        {
            if (name.StartsWith("."))
                throw new ProjectStartsWithDotException();

            var trimmedName = name.Trim();
            AddProject(trimmedName, AbbriviationBuilder.GetAbbriviation(trimmedName));
        }

        //Are we looking at a command here?
        public void AddStory(string projectAbbreviation, string storyTitle)
        {
            var project = GetProject(projectAbbreviation);
            if (project == null)
                throw new ArgumentException(string.Format("Add Story Error: Invalid project abbriviation {0}.",
                                                          projectAbbreviation));

            project.AddStory(storyTitle);
        }

        public void AddStoryFilter(IStoryFilter filter)
        {
            storyFilter = filter;
        }

        //Are we looking at a command here?
        public void Delete(string projectAbbreviation, Story story)
        {
            var project = GetProject(projectAbbreviation);
            if (project == null)
                throw new ArgumentException(string.Format("Delete Story Error: Invalid project abbriviation {0}.",
                                                          projectAbbreviation));
            project.Delete(story);
        }

        public Project GetActiveProject()
        {
            var stories = new List<Story>();
            //TODO: Smelly.
            if (activeProject != null && storyFilter.IsActive)
            {
                foreach (var story in activeProject.Stories)
                    if (storyFilter.Include(story))
                        stories.Add(story);

                var filtered = new Project(activeProject.Name, activeProject.Abbreviation) {Stories = stories.ToArray()};
                return filtered;
            }
            return activeProject;
        }

        public Project GetProject(string abbreviation)
        {
            Project tmp;
            if (_projects.TryGetValue(abbreviation, out tmp))
                return tmp;
            return null;
        }

        public Project[] ListProjects()
        {
            var projects = new List<Project>();
            foreach (var project in _projects.Keys)
                projects.Add(_projects[project]);
            return projects.ToArray();
        }

        public virtual void Load()
        {
            var projects = new Dictionary<string, Project>();
            var root = new DirectoryInfo(persistanceDirectory);
            if (!root.Exists)
            {
                root.Create();
                return;
            }
            foreach (var subdir in root.GetDirectories())
            {
                if (subdir.Name.StartsWith("."))
                    continue;
                foreach (var projectFile in subdir.GetFiles())
                {
                    var project = ProjectFile.Load(projectFile.OpenRead());
                    projects[project.Abbreviation] = project;
                    activeProject = project;
                }
            }
            _projects = projects;
        }

        public virtual bool SaveActiveProject()
        {
            var project = activeProject;
            if (string.IsNullOrEmpty(persistanceDirectory))
                return false;
            var projectOutputDirectory = ProjectFile.CreateHomeDirectory(persistanceDirectory, project);
            var fileName = string.Format("{0}/{1}.xml", projectOutputDirectory, project.Abbreviation);
            ProjectFile.Create(fileName, project);
            return true;
        }

        public void SetActiveProject(string projectName)
        {
            var abbriviation = AbbriviationBuilder.GetAbbriviation(projectName);
            var project = GetProject(abbriviation);
            if (project == null)
                throw new ArgumentException(string.Format("Invalid project name: {0}", projectName));
            activeProject = project;
        }

        public void Update(string projectAbbreviation, Story story)
        {
            var project = GetProject(projectAbbreviation);
            if (project == null)
                throw new ArgumentException(string.Format("Update Story Error: Invalid project abbriviation {0}.",
                                                          projectAbbreviation));

            project.Update(story);
        }

        private void AddProject(string name, string abbreviation)
        {
            if (GetProject(abbreviation) != null)
                throw new ProjectAllreadyExistsException(name);

            activeProject = new Project(name, abbreviation);
            _projects.Add(abbreviation, activeProject);
        }

        private Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        private Project activeProject;
        private string persistanceDirectory;
        private IStoryFilter storyFilter = new StoryFilter();
    }
}