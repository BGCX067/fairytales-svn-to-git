using System;
using ConeFabric.FairyTales.Core;

namespace Conefabric.FairyTales.Web.Controls
{
    public class BacklogPresenter
    {
        public BacklogPresenter(IBacklogView view, ProjectModel model)
            : this(view, model, new TitleAbbreviator(20))
        {
        }

        public BacklogPresenter(IBacklogView view, ProjectModel model, TitleAbbreviator abbreviator)
        {
            this.view = view;
            this.model = model;
            this.abbreviator = abbreviator;
        }

        public void Present()
        {
            var projects = model.ListProjects();
            view.Projects = projects;
            var project = model.GetActiveProject();
            
            if (project != null && project.Stories != null)
            {
                foreach (var story in project.Stories)
                    story.Name = abbreviator.Abbreviate(story.Name);
            }
            view.ActiveProject = project;
            if (view.CreateProjectControlsVisible == false)
                view.CreateProjectControlsVisible = projects.Length == 0;
        }

        public void AddProject(string projectName)
        {
            try
            {
                if (projectName != string.Empty)
                {
                    model.AddProject(projectName);
                    model.SaveActiveProject();
                    view.ErrorMessage = string.Empty;
                }
            }
            catch (Exception e)
            {
                view.ErrorMessage = e.Message;
            }
        }

        public void AddStoryToActiveProject(string storyTitle)
        {
            Project active = model.GetActiveProject();
            model.AddStory(active.Abbreviation, storyTitle);
            model.SaveActiveProject();
            view.ErrorMessage = string.Empty;
        }
        private readonly TitleAbbreviator abbreviator;
        private readonly ProjectModel model;
        private readonly IBacklogView view;

    }
}