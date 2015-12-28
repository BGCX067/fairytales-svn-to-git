using ConeFabric.FairyTales.Core;
using Conefabric.FairyTales.Web.Controls;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class ViewTests
    {
        [Test]
        public void ShouldNotBeEditingByDefault()
        {
            var view = new ProjectView();
            Assert.That(view.IsEditing, Is.False);
        }

        [Test]
        public void ShouldBeAbleToGoIntoEditMode()
        {
            var view = new ProjectView();
            view.Edit();
            Assert.That(view.IsEditing, Is.True);
        }

        [Test]
        public void IsEditAware()
        {
            var view = new ProjectView();
            Assert.That(view.IsEditing, Is.False);
            view.Edit();
            Assert.That(view.IsEditing, Is.True);
            view.StopEditing();
            Assert.That(view.IsEditing, Is.False);
        }

        [Test]
        public void WhenNotEditingTitleAbbreviationShouldShow()
        {
            var view = new ProjectView(new TitleAbbreviator(10));
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("My title that should be abbreviated");
            view.ActiveProject = project;
            Assert.That(view.ActiveProject.Stories[0].Name, Is.EqualTo("My title ..."));
        }
        [Test]
        public void WhenEditingTitleAbbreviationShouldNotShow()
        {
            var view = new ProjectView(new TitleAbbreviator(10));
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("My title that should be abbreviated");
            view.ActiveProject = project;
            view.Edit();
            Assert.That(view.ActiveProject.Stories[0].Name, Is.EqualTo("My title that should be abbreviated"));
        }
    }
}