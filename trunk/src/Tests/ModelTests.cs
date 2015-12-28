using System;
using ConeFabric.FairyTales.Core;
using Conefabric.FairyTales.Web.Controls;
using NUnit.Framework;
using System.IO;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void CantDeleteStoryIfProjectAintFound()
        {
            var model = new ProjectModel();
            model.Delete("Foo", new Story("Test", "Foo-1"));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void CantUpdateStoryIfProjectAintFound()
        {
            var model = new ProjectModel();
            model.Update("Foo", new Story("Test", "Foo-1"));
        }

        [Test]
        public void GetProjectShouldReturnNullIfNoProjectIsFound()
        {
            var model = new ProjectModel();
            Assert.IsNull(model.GetProject("FT"));
        }

        [Test]
        public void ModelShouldBeAbleToAddAProject()
        {
            var model = new ProjectModel();

            Project[] projects = model.ListProjects();
            Assert.AreEqual(0, projects.Length);

            model.AddProject("Fairy Tales");

            projects = model.ListProjects();
            Assert.AreEqual(1, projects.Length);

            Assert.AreEqual("Fairy Tales", projects[0].Name);
            Assert.AreEqual("FT", projects[0].Abbreviation);
        }

        [Test]
        public void ModelShouldBeAbleToAddAStoryToAProject()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");

            model.AddStory("FT", "Add Story Story");
            Project fairyTales = model.GetProject("FT");
            Story[] stories = fairyTales.Stories;
            Assert.AreEqual(1, stories.Length);

            Assert.AreEqual("FT-1", stories[0].Abbreviation);
            Assert.AreEqual("Add Story Story", stories[0].Name);
        }

        [Test]
        public void ModelShouldBeAbleToAddStoriesToDifferentProjects()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddProject("Route Builder");

            model.AddStory("RB", "Add Route");
            model.AddStory("FT", "Add Stories To Different Projects");
            model.AddStory("RB", "Add Route Description");

            Project[] projects = model.ListProjects();

            Assert.AreEqual(2, projects.Length);

            Story[] stories = projects[0].Stories;
            Assert.AreEqual(1, stories.Length);
            Assert.AreEqual("FT-1", stories[0].Abbreviation);

            stories = projects[1].Stories;
            Assert.AreEqual(2, stories.Length);
            Assert.AreEqual("RB-1", stories[0].Abbreviation);
            Assert.AreEqual("RB-2", stories[1].Abbreviation);
        }

        [Test]
        public void ModelShouldKeepProjectNamesUnique()
        {
            var model = new ProjectModel();
            try
            {
                model.AddProject("Fairy Tales");
                model.AddProject("Fairy Tales");
            }
            catch (ProjectAllreadyExistsException paee)
            {
                Assert.AreEqual(
                    "A project with the name <Fairy Tales> allready exists. Please choose another name for your project.",
                    paee.Message);
            }
        }

        [Test]
        public void ModelShouldNotAllowProjectNamesThatStartsWithDots()
        {
            var model = new ProjectModel();
            try
            {
                model.AddProject(".project");
            }
            catch (ProjectStartsWithDotException pswde)
            {
                Assert.AreEqual(
                    "Fairy Tales does not support project names that starts with a dot (.), please choose another.",
                    pswde.Message);
                Assert.AreEqual(0, model.ListProjects().Length);
            }
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void NoStoryShouldBeAddedIfNoProjectIsFound()
        {
            var model = new ProjectModel();
            model.AddStory("Foo", "Story");
        }

        [Test]
        public void ShouldBeAbleToAddAStoryFilterThatFiltersStories()
        {
            var model = new ProjectModel();
            var view = new NullView();
            var presenter = new BacklogPresenter(view, model);

            presenter.AddProject("Fairy Tales");
            presenter.AddStoryToActiveProject("Not Started Story");

            presenter.Present();
            Assert.AreEqual(1, view.ActiveProject.Stories.Length);

            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();
            model.AddStoryFilter(filter);
            presenter.Present();
            Assert.AreEqual(0, view.ActiveProject.Stories.Length);
        }

        [Test]
        public void ShouldBeAbleToFilterOutStories()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Filter Me!");

            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();

            model.AddStoryFilter(filter);
            var activeProject = model.GetActiveProject();
            Assert.AreEqual(0, activeProject.Stories.Length);
        }

        [Test]
        public void ShouldOnlyFilterSpecifiedStories()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Don't Filter Me!");

            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryDone>();

            model.AddStoryFilter(filter);
            var activeProject = model.GetActiveProject();
            Assert.AreEqual(1, activeProject.Stories.Length);
        }

        [Test]
        public void StoriesCanBeRemovedFromAProject()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Delete Story Story");

            Project fairyTales = model.GetProject("FT");
            Story story = fairyTales.Stories[0];

            model.Delete(fairyTales.Abbreviation, story);

            fairyTales = model.GetProject("FT");
            Assert.AreEqual(0, fairyTales.Stories.Length);
        }

        [Test]
        public void StoryNamesShouldBeUpdateable()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Foo Bar Story");

            Project fairyTales = model.GetProject("FT");
            Story story = fairyTales.Stories[0];
            story.Name = "Update Story Story";

            model.Update(fairyTales.Abbreviation, story);

            story = fairyTales.Stories[0];
            Assert.AreEqual("Update Story Story", story.Name);
        }

        [Test]
        public void SwitchingProjectShouldSetActiveProject()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddProject("Troy");

            Assert.AreEqual("Troy", model.GetActiveProject().Name);
            model.SetActiveProject("Fairy Tales");
            Assert.AreEqual("Fairy Tales", model.GetActiveProject().Name);
        }

        [Test]
        public void AddProjectShouldNotCreateADirectoryIfNoPersistanceDirIsSet()
        {
            var model = new ProjectModel();
            var view = new NullView();
            var presenter = new BacklogPresenter(view, model);
            presenter.AddProject("Fairy Tales");
            Assert.IsFalse(Directory.Exists("c:\\Fairy Tales"));
        }

        [Test]
        public void SaveProjectShouldNotCrashIfPersistanceDirIsNotSet()
        {
            var model = new ProjectModel();
            Assert.IsFalse(model.SaveActiveProject());
        }

        [Test]
        public void SaveProjectShouldNotCrashIfPersistanceDirIsEmpty()
        {
            var model = new ProjectModel {PersistanceDirectory = string.Empty};
            Assert.IsFalse(model.SaveActiveProject());
        }
    }
}