using System;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class DomainObjectTests
    {
        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ProjectWillTrowExceptionOnDeleteWhenStoryDontExist()
        {
            var project = new Project("Fairy Tales", "FT");
            project.Delete(new Story("Undefined Story", "FT-1"));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void UnconnectedStoriesCantBeUpdated()
        {
            var project = new Project("Fairy Tales", "FT");
            var story = new Story("UnConnected", "FT-1");
            project.Update(story);
        }
    }
}