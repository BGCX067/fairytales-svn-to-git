using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConeFabric.FairyTales.Core;
using Conefabric.FairyTales.Web.Controls;

namespace Web.FairyTales
{
    public partial class ProjectPage : Page, IBacklogView
    {
        public Project ActiveProject
        {
            set
            {
                if (projectChooser.Items.Count == 0)
                {
                    projectChooser.SelectedIndex = 0;
                    return;
                }

                var index = 0;
                foreach (ListItem project in projectChooser.Items)
                {
                    if (project.Text == value.Name)
                        break;
                    index++;
                }
                projectChooser.SelectedIndex = index;
                PresentStories(value);
            }
        }

        private ProjectModel BacklogModel { get { return Session["ProjectModel"] as ProjectModel; } set { Session["ProjectModel"] = value; } }

        public bool CreateProjectControlsVisible { get { return createProjectControls.Visible; } set { createProjectControls.Visible = value; } }
        public string ErrorMessage { set { errorMessageLabel.Text = value; } }
        public bool IsEditing { get; private set; }
        private BacklogPresenter Presenter { get { return new BacklogPresenter(this, BacklogModel); } }

        private static string ProjectOutputDirectory { get { return ConfigurationManager.AppSettings["ProjectOutputDir"]; } }

        public Project[] Projects
        {
            set
            {
                var collection = new ListItemCollection();

                foreach (var project in value)
                    collection.Add(new ListItem(project.Name, project.Abbreviation));

                projectChooser.DataSource = collection;
                projectChooser.DataBind();
                SetStoryWidgetsEnableTo(collection.Count != 0);
            }
        }

        protected static StoryStatusChange[] StoryStatuses
        {
            get
            {
                return new StoryStatusChange[]
                           {new StoryNotStarted(), new StoryStarted(), new StoryPaused(), new StoryDone()};
            }
        }

        public void Edit()
        {
            IsEditing = true;
        }

        public void StopEditing()
        {
            storyTable.EditIndex = -1;
            IsEditing = false;
            Presenter.Present();
        }

        protected static string GetClassByStatus(Story story)
        {
            if (story.IsStarted)
                return "started";
            if (story.IsPaused)
                return "paused";
            return story.IsDone ? "done" : "notStarted";
        }

        protected void addProjectButton_Click(object sender, EventArgs e)
        {
            Presenter.AddProject(projectInput.Text);
            projectInput.Text = string.Empty;
            storyInput.Focus();
            Presenter.Present();
        }

        protected void addStoryButton_Click(object sender, EventArgs e)
        {
            if (storyInput.Text != string.Empty)
            {
                Presenter.AddStoryToActiveProject(storyInput.Text);
                FocusAndClearStoryInput();
            }

            Presenter.Present();
        }

        protected void BindRow(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var story = e.Row.DataItem as Story;
            if (story != null)
                e.Row.CssClass = GetClassByStatus(story);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BacklogModel == null)
            {
                BacklogModel = SetupModel();
                Presenter.Present();
                projectInput.Focus();
            }
            else if (!IsPostBack)
                Presenter.Present();

            //TODO
            //storyInput.Attributes.Add("onfocus", string.Format("javascript:setSubmitFocus('{0}')", addStoryButton.ClientID));
        }

        protected void projectChooserIndexChanged(object sender, EventArgs args)
        {
            var dropdown = sender as DropDownList;
            if (dropdown != null)
                BacklogModel.SetActiveProject(dropdown.SelectedValue);
            Presenter.Present();
        }

        protected void storyTable_CancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StopEditing();
        }

        protected void storyTable_DeleteRow(object sender, EventArgs e)
        {
            var control = sender as LinkButton;
            if (control == null)
                return;
            var abbreviation = control.CommandArgument;

            Story storyToDelete = null;
            var active = BacklogModel.GetActiveProject();
            foreach (var story in active.Stories)
                if (story.Abbreviation == abbreviation)
                {
                    storyToDelete = story;
                    break;
                }
            if (storyToDelete == null)
                return;

            BacklogModel.Delete(active.Abbreviation, storyToDelete);
            BacklogModel.SaveActiveProject();
            Presenter.Present();
        }

        protected void storyTable_EditRow(object sender, GridViewEditEventArgs e)
        {
            storyTable.EditIndex = e.NewEditIndex;
            Presenter.Present();
        }

        protected void storyTable_FilterStories(object sender, EventArgs args)
        {
            var filters = sender as CheckBoxList;
            var filter = new StoryFilter();
            if (filters != null)
                for (var i = 0; i < 4; ++i)
                    if (filters.Items[i].Selected)
                        filter.AddStoryFilter(StoryStatuses[i].GetType());
                    else
                        filter.RemoveStoryFilter(StoryStatuses[i].GetType());
            BacklogModel.AddStoryFilter(filter);
            Presenter.Present();
        }

        protected void storyTable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            storyTable.PageIndex = e.NewPageIndex;
            Presenter.Present();
        }

        protected void storyTable_UpdateRow(object sender, GridViewUpdateEventArgs e)
        {
            var row = storyTable.Rows[e.RowIndex];

            Story storyToUpdate;
            var project = FindTheEditingStory(e.RowIndex, out storyToUpdate);

            var newTitle = row.FindControl("storyName_edit") as TextBox;
            var newImportance = row.FindControl("importance_edit") as TextBox;
            var newEstimate = row.FindControl("estimate_edit") as TextBox;
            var newStatus = row.FindControl("status_dropdown") as DropDownList;
            var newHowToDemo = row.FindControl("howToDemo_edit") as TextBox;

            if (newTitle != null)
                storyToUpdate.Name = newTitle.Text;
            if (newImportance != null)
                storyToUpdate.Importance = Convert.ToInt32(newImportance.Text);
            if (newEstimate != null)
                storyToUpdate.Estimate = Convert.ToInt32(newEstimate.Text);
            if (newHowToDemo != null)
                storyToUpdate.HowToDemo = newHowToDemo.Text;

            if (newStatus != null)
            {
                var status = (StoryStatus) newStatus.SelectedIndex;
                switch (status)
                {
                    case StoryStatus.NotStarted:
                        storyToUpdate.Reset();
                        break;
                    case StoryStatus.Paused:
                        storyToUpdate.Pause();
                        break;
                    case StoryStatus.Started:
                        storyToUpdate.Start(DateTime.Now);
                        break;
                    case StoryStatus.Done:
                        storyToUpdate.MarkAsDone(DateTime.Now);
                        break;
                }
            }

            BacklogModel.Update(project.Abbreviation, storyToUpdate);

            BacklogModel.SaveActiveProject();
            StopEditing();
        }

        protected void ToggleCreateProjectControlsVisibility(object sender, EventArgs args)
        {
            CreateProjectControlsVisible = !CreateProjectControlsVisible;
        }

        private static ProjectModel SetupModel()
        {
            var model = ProjectOutputDirectory == "Memory" ? new InMemoryModel() : new ProjectModel();
            model.PersistanceDirectory = ProjectOutputDirectory;
            model.Load();

            return model;
        }

        private Project FindTheEditingStory(int rowIndex, out Story editingStory)
        {
            var abbreviation = storyTable.Rows[rowIndex].Cells[1].Text;
            var project = BacklogModel.GetActiveProject();
            editingStory = null;

            foreach (var story in project.Stories)
                if (story.Abbreviation == abbreviation)
                {
                    editingStory = story;
                    break;
                }
            return project;
        }

        private void FocusAndClearStoryInput()
        {
            storyInput.Text = "";
            storyInput.Focus();
        }

        private void PresentStories(Project project)
        {
            activeProjectLabel.Text = project.Name;
            storyTable.DataSource = project.ListStoriesByImportance(false);
            storyTable.DataBind();
        }

        private void SetStoryWidgetsEnableTo(bool what)
        {
            storyInput.Enabled = what;
            addStoryButton.Enabled = what;
        }
    }
}