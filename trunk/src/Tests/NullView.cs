using System.Collections.Generic;
using ConeFabric.FairyTales.Core;
using Conefabric.FairyTales.Web.Controls;

namespace ConeFabric.FairyTales.Tests
{
    internal class NullView : IBacklogView
    {
        private string error = string.Empty;
        private List<Project> projects = new List<Project>();

        public Project[] Projects
        {
            set { projects.AddRange(value); }
        }

        public Project ActiveProject { set; get; }

        public string ErrorMessage
        {
            get { return error; }
            set { error = value; }
        }

        public void SetProjects(List<Project> list)
        {
            projects = list;
        }

        public bool CreateProjectControlsVisible { get; set; }

        public void Edit()
        {
            IsEditing = true;
        }

        public void StopEditing()
        {
            IsEditing = false;
        }

        public bool IsEditing { get; set; }
    }
}