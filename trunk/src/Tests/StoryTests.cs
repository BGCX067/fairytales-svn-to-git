using System;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class StoryTests
    {
        [Test]
        public void ImportanceCanBeSetOnAStory()
        {
            var story = new Story("Set importance story", "FT-1") {Importance = 42};
            Assert.AreEqual(42, story.Importance);
        }

        [Test]
        public void SettingAStoryToDoneShouldCalculatesRealTimeSpent()
        {
            var story = new Story("Stalled story", "FT-1");
            var startDate = new DateTime(2008, 04, 21);
            var endDate = new DateTime(2008, 04, 23);
            story.Start(startDate);
            story.MarkAsDone(endDate);
            Assert.AreEqual(new TimeSpan(2, 0, 0, 0), story.Duration);
            Assert.IsTrue(story.IsDone);
        }

        [Test]
        public void StatusChangesAreTracked()
        {
            var story = new Story("Changing Story", "CS-1");
            var startDate = new DateTime(2008, 08, 16);
            var pauseDate = new DateTime(2008, 08, 17);
            var reOpenDate = new DateTime(2008, 08, 18);
            var endDate = new DateTime(2008, 08, 19);
            story.Start(startDate);
            story.Pause(pauseDate);
            story.Start(reOpenDate);
            story.MarkAsDone(endDate);

            Assert.AreEqual(4, story.StatusChange.Length);
            Assert.IsInstanceOfType(typeof(StoryStarted), story.StatusChange[0]);
            Assert.AreEqual(startDate, story.StatusChange[0].Date);

            Assert.IsInstanceOfType(typeof(StoryPaused), story.StatusChange[1]);
            Assert.AreEqual(pauseDate, story.StatusChange[1].Date);

            Assert.IsInstanceOfType(typeof(StoryStarted), story.StatusChange[2]);
            Assert.AreEqual(reOpenDate, story.StatusChange[2].Date);

            Assert.IsInstanceOfType(typeof(StoryDone), story.StatusChange[3]);
            Assert.AreEqual(endDate, story.StatusChange[3].Date);
        }

        [Test]
        public void StoriesShouldBeAbleToBePaused()
        {
            var story = new Story("Stalled story", "FT-1");
            story.Pause();
            Assert.IsTrue(story.IsPaused);
        }

        [Test]
        public void StoriesShouldBeAbleToGetStarted()
        {
            var story = new Story("Started story", "FT-1");
            DateTime startTime = DateTime.Now;
            story.Start(startTime);

            Assert.IsTrue(story.IsStarted);
            Assert.AreEqual(startTime, story.StartTime);
        }

        [Test]
        public void StoriesShouldHaveZeroAsDefaultImportance()
        {
            var story = new Story("Importance Story", "FT-1");
            Assert.AreEqual(0, story.Importance);
        }

        [Test]
        public void StoryShouldHaveACreationTime()
        {
            var story = new Story("Created Now Story", "FT-1");
            Assert.AreEqual(DateTime.Today.Date, story.Created.Date);
        }

        [Test]
        public void StoryStatusShouldDefaultToNotStarted()
        {
            var story = new Story("Story status story", "FT-1");
            Assert.IsFalse(story.IsStarted);
        }
        
        [Test]
        public void SettingStoryToDoneGivesMinusOneImportance()
        {
            var story = new Story("Story Done Thingy", "FT-1");
            story.MarkAsDone(DateTime.Now);
            Assert.AreEqual(-1, story.Importance);
        }

        [Test]
        public void ReopeningStoryShouldRestoreImportance()
        {
            var story = new Story("Reopen Story Thigy", "FT-1") {Importance = 42};
            story.MarkAsDone(DateTime.Now);
            story.Start(DateTime.Now);
            Assert.AreEqual(42, story.Importance);
        }

        [Test]
        public void MinusOneShouldBeAbleToBeCrowded()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Done Story");
            project.AddStory("Other Story");

            project.Stories[0].MarkAsDone(DateTime.Now);
            project.Stories[1].MarkAsDone(DateTime.Now);

            Assert.AreEqual(-1, project.Stories[0].Importance);
            Assert.AreEqual(-1, project.Stories[1].Importance);
        }

        [Test]
        public void ShouldBeAbleToResetStory()
        {
            var story = new Story("Not Started", "FT-1");
            story.Start(DateTime.Now);
            story.Reset();
            Assert.IsFalse(story.IsStarted);
        }
        
        [Test]
        public void ResettedStoriesShouldNotBeDone()
        {
            var story = new Story("Not Done", "FT-1");
            story.Start(DateTime.Now);
            story.MarkAsDone(DateTime.Now);
            story.Reset();
            Assert.IsFalse(story.IsDone);
        }

        [Test]
        public void DoublePostingResetShouldNotAffectStatusChange()
        {
            var story = new Story("Not Started", "FT-1");
            story.Reset();
            Assert.AreEqual(0, story.StatusChange.Length);
        }

        [Test]
        public void DoublePostingStartShouldNotAffectStatusChange()
        {
            var story = new Story("Started Once", "FT-1");
            story.Start(DateTime.Now);
            story.Start(DateTime.Now);
            Assert.AreEqual(1, story.StatusChange.Length);
        }

        [Test]
        public void DoublePostingPauseShouldNotAffectStatusChange()
        {
            var story = new Story("Paused Once", "FT-1");
            story.Pause();
            story.Pause();
            Assert.AreEqual(1, story.StatusChange.Length);
        }

        [Test]
        public void DoublePostingMarkAdDoneShouldNotAffectStatusChange()
        {
            var story = new Story("Done Once", "FT-1");
            story.MarkAsDone(DateTime.Now);
            story.MarkAsDone(DateTime.Now);
            Assert.AreEqual(1, story.StatusChange.Length);
        }
    }
}