
using NUnit.Framework;
using ConeFabric.FairyTales.Core;
using System.IO;
using System;
namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class ProjectFileTests
    {
        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void CreateHomeDirectoryShouldNotCreateIfNoRootDir()
        {
            ProjectFile.CreateHomeDirectory(string.Empty, new Project("No Project", "NP"));
            Assert.Fail("Should not get here");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateHomeDirectoryShouldNotCreateIfNoProjectName()
        {
            ProjectFile.CreateHomeDirectory("Temp", new Project("", ""));
            Assert.Fail("Should not get here");
        }
        [Test]
        [ExpectedException(typeof(InvalidProjectStreamException))]
        public void LoadShouldThrowOnInvalidStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Nothing regular really");
            ProjectFile.Load(stream);
        }

        [TearDown]
        public void CleanUp()
        {
            var tmp = new DirectoryInfo(ModelPersistanceTests.ProjectDir);
            if (tmp.Exists)
                tmp.Delete(true);
        }

        [Test]
        public void CreateShouldNotCrashOnProjectSave()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("First");
            project.Stories[0].Start(DateTime.Today);
            project.Stories[0].MarkAsDone(DateTime.Today);

            var projectOutputDirectory = ProjectFile.CreateHomeDirectory(ModelPersistanceTests.ProjectDir, project);
            var file = string.Format("{0}/{1}.xml", projectOutputDirectory, project.Abbreviation);
            ProjectFile.Create(file, project);

            Assert.IsTrue(Directory.Exists(ModelPersistanceTests.ProjectDir));
            Assert.IsTrue(File.Exists(file));
        }
    }
}
