using System;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void ImportanceCanPushOtherStoriesForwardOnInsert()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Story 1");
            project.AddStory("Story 2");
            project.AddStory("Story 3");

            project.Stories[0].Importance = 100;
            project.Stories[1].Importance = 101;
            project.Stories[2].Importance = 100;

            Assert.AreEqual(101, project.Stories[0].Importance);
            Assert.AreEqual(102, project.Stories[1].Importance);
            Assert.AreEqual(100, project.Stories[2].Importance);
        }

        [Test]
        public void ProjectShouldBeAbleToOwnStoriesOnLoad()
        {
            var project = new Project("Fairy Tales", "FT")
                              {
                                  Stories = new[] {new Story("First story", "FT-1")}
                              };
            project.AddStory("Second Story");
            Assert.AreEqual("FT-2", project.Stories[1].Abbreviation);
        }

        [Test]
        public void ProjectShouldBeAbleToSortStoriesByImportanceDesc()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Low Importance Story");
            project.AddStory("High Importance Story");
            project.AddStory("Middle Importance Story");

            project.Stories[0].Importance = 0;
            project.Stories[1].Importance = 100;
            project.Stories[2].Importance = 10;

            Story[] stories = project.ListStoriesByImportance(false);
            Assert.AreEqual("High Importance Story", stories[0].Name);
            Assert.AreEqual("Middle Importance Story", stories[1].Name);
            Assert.AreEqual("Low Importance Story", stories[2].Name);
        }

        [Test]
        public void ProjectShouldConsiderDeletedStories()
        {
            var project = new Project("Fairy Tales", "FT")
                              {
                                  Stories = new[] {new Story("Second Story", "FT-2")}
                              };
            project.AddStory("Third Story");
            Assert.AreEqual("FT-3", project.Stories[1].Abbreviation);
        }

        [Test]
        public void ProjectShouldTakeOwnerShipOfStoriesOnLoad()
        {
            var project = new Project("Fairy Tales", "FT")
                              {
                                  Stories = new[] {new Story("First story", "FT-1")}
                              };
            Assert.AreSame(project, project.Stories[0].Owner);
        }

        [Test]
        public void SettingTheSameImportanceAsAnotherStoryAdds1()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Story 1");
            project.AddStory("Story 2");

            project.Stories[0].Importance = 100;
            project.Stories[1].Importance = 100;

            Assert.AreEqual(101, project.Stories[0].Importance);
            Assert.AreEqual(100, project.Stories[1].Importance);
        }

        [Test]
        public void ZeroImportanceShouldNotPushOtherStories()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("First Story");
            project.AddStory("Second Story");

            Story first = project.Stories[0];
            first.Name = "First again";
            first.Importance = 0;
            project.Update(first);
            Assert.AreEqual(0, project.Stories[0].Importance);
            Assert.AreEqual(0, project.Stories[1].Importance);
        }
    }
}