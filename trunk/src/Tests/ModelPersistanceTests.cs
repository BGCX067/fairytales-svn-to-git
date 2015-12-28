using System;
using System.IO;
using System.Xml.Serialization;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class ModelPersistanceTests
    {
        [TearDown]
        public void CleanUp()
        {
            var tmp = new DirectoryInfo(ProjectDir);
            if (tmp.Exists)
                tmp.Delete(true);
        }

        public const string ProjectDir = "./Projects";

        [Test]
        public void ModelShouldBeAbleToCreatePersistanceOutputFile()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");

            model.PersistanceDirectory = ProjectDir;
            model.SaveActiveProject();

            var file = new FileInfo(ProjectDir + "/Fairy Tales/FT.xml");
            Assert.IsTrue(file.Exists);
        }

        [Test]
        public void ModelShouldBeAbleToLoadProjectsFromPersistance()
        {
            var model = new ProjectModel { PersistanceDirectory = ProjectDir };
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Load from persistance story");
            model.SaveActiveProject();

            model = new ProjectModel {PersistanceDirectory = ProjectDir};
            model.Load();

            var project = model.GetActiveProject();
            Assert.AreEqual("Fairy Tales", project.Name);
            Assert.AreEqual(1, project.Stories.Length);
        }

        [Test]
        public void ModelShouldBeAbleToStoreStoryDataWhenSaving()
        {
            const string projectName = "Fairy Tales";

            var model = new ProjectModel();
            model.AddProject(projectName);
            model.AddStory("FT", "Model should be able to store story data when saving");

            model.PersistanceDirectory = ProjectDir;
            model.SaveActiveProject();

            var file = new FileInfo(ProjectDir + "/" + projectName + "/FT.xml");

            var serializer = new XmlSerializer(typeof (Project));

            var reader = new StreamReader(file.OpenRead());
            var fileContent = reader.ReadToEnd();
            reader.Close();

            var project = model.GetProject("FT");
            var deserializedProject = (Project) serializer.Deserialize(new StringReader(fileContent));
            Assert.AreEqual(project.Name, deserializedProject.Name);
            Assert.AreEqual(project.Abbreviation, deserializedProject.Abbreviation);

            Assert.AreEqual(project.Stories.Length, deserializedProject.Stories.Length);
            for (var i = 0; i < project.Stories.Length; ++i)
            {
                Assert.AreEqual(project.Stories[i].Name, deserializedProject.Stories[i].Name);
                Assert.AreEqual(project.Stories[i].Abbreviation, deserializedProject.Stories[i].Abbreviation);
            }
        }

        [Test]
        public void ModelShouldCreateProjectDirectoryIfNotPresent()
        {
            var model = new ProjectModel {PersistanceDirectory = ProjectDir};

            var directory = new DirectoryInfo(ProjectDir);
            Assert.IsFalse(directory.Exists);

            model.Load();
            directory = new DirectoryInfo(ProjectDir);
            Assert.IsTrue(directory.Exists);
        }

        [Test]
        public void ModelShouldLoadProjects()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddProject("Troy");

            model.PersistanceDirectory = ProjectDir;
            model.SaveActiveProject();
            model.SetActiveProject("Fairy Tales");
            model.SaveActiveProject();

            model = new ProjectModel {PersistanceDirectory = ProjectDir};
            model.Load();
            Project active = model.GetActiveProject();

            Assert.AreEqual("Troy", active.Name);
            Assert.IsNotNull(model.GetProject("FT"));
        }

        [Test]
        public void ModelShouldTrunkateFileWhenSerailizeing()
        {
            var model = new ProjectModel();
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Delete Story");

            model.PersistanceDirectory = ProjectDir;
            model.SaveActiveProject();

            var active = model.GetActiveProject();
            active.Delete("FT-1");

            model.SaveActiveProject();

            model.Load();
            active = model.GetActiveProject();
            Assert.AreEqual(0, active.Stories.Length);
        }

        [Test]
        public void ShouldNotFilterOnSave()
        {
            var model = new ProjectModel();

            model.AddProject("Fairy Tales");
            model.AddStory("FT", "Not Started");
            var filter = new StoryFilter();
            filter.AddStoryFilter<StoryNotStarted>();
            model.AddStoryFilter(filter);

            model.PersistanceDirectory = ProjectDir;
            model.SaveActiveProject();

            model = new ProjectModel {PersistanceDirectory = ProjectDir};
            model.Load();

            var active = model.GetActiveProject();
            Assert.AreEqual(1, active.Stories.Length);
        }

        [Test]
        public void ShouldBeAbleToSaveProjectWithStatusChanges()
        {
            var model = new ProjectModel
                            {
                                PersistanceDirectory = ProjectDir
                            };
            model.AddProject("Fairy Tales");
            model.AddStory("FT", "First");
            var active = model.GetActiveProject();
            active.Stories[0].Start(DateTime.Today);
            active.Stories[0].Importance = 42;
            active.Stories[0].MarkAsDone(DateTime.Today);

            model.SaveActiveProject();

            model = new ProjectModel
                        {
                            PersistanceDirectory = ProjectDir
                        };
            model.Load();
            active = model.GetActiveProject();
            Assert.AreEqual("Fairy Tales", active.Name);
            Assert.AreEqual("FT", active.Abbreviation);
            Assert.AreEqual(1, active.Stories.Length);
            Assert.IsTrue(active.Stories[0].IsDone);
        }
    }
}
