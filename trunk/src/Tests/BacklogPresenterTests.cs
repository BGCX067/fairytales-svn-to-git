using ConeFabric.FairyTales.Core;
using Conefabric.FairyTales.Web.Controls;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class BacklogPresenterTests
    {
        [Test]
        public void ActiveProjectShouldBeSetAfterPresent()
        {
            var view = new NullView();

            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddProject("Troy");

            var presenter = new BacklogPresenter(view, model);
            presenter.Present();
            Assert.AreEqual("Troy", model.GetActiveProject().Name);
            Assert.AreEqual(view.ActiveProject.Name, model.GetActiveProject().Name);
        }

        [Test]
        public void PresenterShouldClearErrorMessageOnTwoContinousPresents()
        {
            var model = new ProjectModel();
            var presenter = new BacklogPresenter(new NullView(), model);

            presenter.AddProject(".project");
            presenter.Present();

            var view = new NullView();
            presenter = new BacklogPresenter(view, model);
            presenter.Present();
            Assert.AreEqual(string.Empty, view.ErrorMessage);
        }

        [Test]
        public void PresenterShouldNotifyOnDuplicateProjectNames()
        {
            var view = new NullView();

            var model = new ProjectModel();
            var presenter = new BacklogPresenter(view, model);

            presenter.AddProject("Fairy Tales");
            presenter.AddProject("Fairy Tales");
            presenter.Present();

            Assert.AreEqual(
                "A project with the name <Fairy Tales> allready exists. Please choose another name for your project.",
                view.ErrorMessage);
        }

        [Test]
        public void PresenterShouldNotifyOnProjectNamesThatStartWithADot()
        {
            var view = new NullView();

            var model = new ProjectModel();
            var presenter = new BacklogPresenter(view, model);
            presenter.AddProject(".project");
            presenter.Present();
            Assert.AreEqual(
                "Fairy Tales does not support project names that starts with a dot (.), please choose another.",
                view.ErrorMessage);
        }

        [Test]
        public void PresenterShouldTrimProjectNamesUponCreation()
        {
            var model = new ProjectModel();
            var presenter = new BacklogPresenter(new NullView(), model);

            presenter.AddProject(" test project ");

            Project active = model.GetActiveProject();
            Assert.AreEqual("TP", active.Abbreviation);
            Assert.AreEqual("test project", active.Name);
        }

        [Test]
        public void ProjectsShouldBeSetAfterPresent()
        {
            var view = new NullView();

            var model = new ProjectModel();
            model.AddProject("Fairy Tales");

            var presenter = new BacklogPresenter(view, model);
            presenter.Present();
            Assert.AreEqual(1, model.ListProjects().Length);
        }
        [Test]
        public void ShouldShowCreateProjectControlsIfNoProjectAvailable()
        {
            var view = new NullView();

            var presenter = new BacklogPresenter(view, new ProjectModel());

            view.CreateProjectControlsVisible = false;
            presenter.Present();
            Assert.IsTrue(view.CreateProjectControlsVisible);
        }
        [Test]
        public void ShouldNotShowCreateProjectControlsIfProjectsArePresent()
        {
            var view = new NullView();
            var model = new ProjectModel();
            model.AddProject("My Project");

            var presenter = new BacklogPresenter(view, model);

            view.CreateProjectControlsVisible = false;
            presenter.Present();
            Assert.IsFalse(view.CreateProjectControlsVisible);
        }
        [Test]
        public void ShouldNotHideCreateProjectControlsIfVisibleAndProjectsPresent()
        {
            var view = new NullView();
            var model = new ProjectModel();
            model.AddProject("My Project");

            var presenter = new BacklogPresenter(view, model);

            view.CreateProjectControlsVisible = true;
            presenter.Present();
            Assert.IsTrue(view.CreateProjectControlsVisible);
        }

        [Test]
        public void ShouldConstraintStoryTitlesToXCharactersInStoryList()
        {
            var view = new NullView();
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "A story with quite long title that should be abbreviated when presented.");
            var presenter = new BacklogPresenter(view, model, new TitleAbbreviator(35));
            presenter.Present();

            var story = view.ActiveProject.Stories[0];
            Assert.That(story.Name, Is.EqualTo("A story with quite long title that ..."));
        }
    }
}