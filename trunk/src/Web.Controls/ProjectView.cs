using ConeFabric.FairyTales.Core;

namespace Conefabric.FairyTales.Web.Controls
{
    public class ProjectView : System.Web.UI.Page, IBacklogView
    {
        public ProjectView(TitleAbbreviator abbreviator)
        {
            this.abbreviator = abbreviator;
        }

        public ProjectView() : this(new TitleAbbreviator(30))
        {
        }

        public Project ActiveProject
        {
            set { active = value; }
            get
            {
                if(active == null)
                    return null;
                var project = new Project(active.Name, active.Abbreviation);
                foreach (var story in active.Stories)
                {
                    var title = (IsEditing) ? story.Name : abbreviator.Abbreviate(story.Name);
                    project.AddStory(title);
                }
                return project;
            }
        }

        public bool CreateProjectControlsVisible { get; set; }
        public string ErrorMessage { set; get; }
        public bool IsEditing { get; set; }
        public Project[] Projects
        {
            set;
            get;
        }

        public void Edit()
        {
            IsEditing = true;
        }

        public void StopEditing()
        {
            IsEditing = false;
        }

        private Project active;
        private readonly TitleAbbreviator abbreviator;
    }
}