
using System;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class StoryFilterTests
    {
        [Test]
        public void AddNotStartedShouldFilterOutNotStartedStories()
        {
            var story = new Story("Not Started Story", "FT-1");
            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();
            Assert.IsFalse(filter.Include(story));
        }
        [Test]
        public void RemoveStoryFilterShouldIncludeSuchStory()
        {
            var story = new Story("Not Started Story", "FT-1");
            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();
            
            filter.RemoveStoryFilter<StoryNotStarted>();
            Assert.IsTrue(filter.Include(story));
        }
        [Test]
        public void EmptyFilterShouldIncludeAll()
        {
            var story = new Story("Not Started Story", "FT-1");
            var filter = new StoryFilter();
            Assert.IsTrue(filter.Include(story));
        }

        [Test]
        public void ShouldOnlyFilterAddedStatuses()
        {
            var started = new Story("Started", "FT-2");
            started.Start(DateTime.Now);
            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();
            Assert.IsTrue(filter.Include(started));
        }
    }
}

