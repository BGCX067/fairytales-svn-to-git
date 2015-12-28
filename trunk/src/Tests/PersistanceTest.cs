using System;
using System.IO;
using System.Xml.Serialization;
using Cint.XmlAsserter.Core;
using ConeFabric.FairyTales.Core;
using NUnit.Framework;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class PersistanceTest
    {
        private const string expectedStory = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                        <Story xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                          <Abbreviation>FT-1</Abbreviation>
                                          <Name>Serialize Story</Name>
                                          <Importance>-1</Importance>
                                          <Estimate>3</Estimate>
                                          <StartTime>2008-04-19T00:00:00</StartTime>
                                          <Created>2008-04-19T00:00:00</Created>
                                          <HowToDemo>Story ends up in project xml file</HowToDemo>
                                          <StatusChange>
                                            <StoryStatusChange xsi:type=""StoryStarted""><Date>2008-01-23T00:00:00</Date></StoryStatusChange>
                                            <StoryStatusChange xsi:type=""StoryPaused""><Date>2008-01-25T00:00:00</Date></StoryStatusChange>
                                            <StoryStatusChange xsi:type=""StoryNotStarted""><Date>2008-04-19T00:00:00</Date></StoryStatusChange>
                                            <StoryStatusChange xsi:type=""StoryStarted""><Date>2008-04-19T00:00:00</Date></StoryStatusChange>
                                            <StoryStatusChange xsi:type=""StoryDone""><Date>2008-04-24T00:00:00</Date><Importance>42</Importance></StoryStatusChange>
                                          </StatusChange>
                                        </Story>";

        private const string firstStory = @"<Story>
                                <Abbreviation>FT-1</Abbreviation>
                                <Name>Serializable Project And Story</Name>
                                <Importance>11</Importance>
                                <Estimate>0</Estimate>
                                <StartTime>0001-01-01T00:00:00</StartTime>
                                <Created />
                                <StatusChange />
                            </Story>";

        private const string secondStory = @"<Story>
                                <Abbreviation>FT-2</Abbreviation>
                                <Name>Serializable Project And Stories</Name>
                                <Importance>0</Importance>
                                <Estimate>0</Estimate>
                                <StartTime>0001-01-01T00:00:00</StartTime>
                                <Created />
                                <StatusChange />
                            </Story>";

        [Test]
        public void AProjectWithServeralStoriesCanBeSerialized()
        {
            string expected =
                string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Project xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                  <Abbreviation>FT</Abbreviation>
                                  <Name>Fairy Tales</Name>
                                  <Stories>
                                    {0}
                                    {1}
                                  </Stories>
                                </Project>",
                    firstStory, secondStory);

            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Serializable Project And Story");
            project.AddStory("Serializable Project And Stories");
            project.Stories[0].Importance = 11;

            var serializer = new XmlSerializer(typeof (Project));
            var writer = new StringWriter();
            serializer.Serialize(writer, project);
            XmlAssert.AreEqual(expected, writer.GetStringBuilder().ToString());
        }

        [Test]
        public void ProjectIsSerializeable()
        {
            const string expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Project xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                  <Abbreviation>FT</Abbreviation>
                                  <Name>Fairy Tales</Name>
                                  <Stories />
                                </Project>";
            var project = new Project("Fairy Tales", "FT");
            var serializer = new XmlSerializer(typeof (Project));
            var writer = new StringWriter();
            serializer.Serialize(writer, project);
            XmlAssert.AreEqual(expected, writer.GetStringBuilder().ToString());
        }

        [Test]
        public void ProjectWithStoryIsSerializable()
        {
            string expected =
                string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Project xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                  <Abbreviation>FT</Abbreviation>
                                  <Name>Fairy Tales</Name>
                                  <Stories>
                                    {0}
                                  </Stories>
                                </Project>",
                    firstStory);

            var project = new Project("Fairy Tales", "FT");
            project.AddStory("Serializable Project And Story");
            project.Stories[0].Importance = 11;

            var serializer = new XmlSerializer(typeof (Project));
            var writer = new StringWriter();
            serializer.Serialize(writer, project);
            XmlAssert.AreEqual(expected, writer.GetStringBuilder().ToString());
        }

        [Test]
        public void SerializationRoundtrip()
        {
            var project = new Project("Fairy Tales", "FT");
            project.AddStory("First Story");
            project.AddStory("Another Story");
            project.AddStory("That Story");

            project.Stories[0].Importance = 10;
            project.Stories[1].Estimate = 3;
            project.Stories[1].Start(DateTime.Now);
            project.Stories[2].MarkAsDone(DateTime.Now);
            project.Stories[2].HowToDemo = "Aother story demo";

            Assert.AreEqual("First Story", project.Stories[0].Name);

            var serializer = new XmlSerializer(typeof (Project));
            var writer = new StringWriter();
            serializer.Serialize(writer, project);
            string serializedProject = writer.GetStringBuilder().ToString();
            writer.Close();

            var deserializedProject = (Project) serializer.Deserialize(new StringReader(serializedProject));
            Assert.AreEqual(project.Name, deserializedProject.Name);
            Assert.AreEqual(project.Abbreviation, deserializedProject.Abbreviation);

            Assert.AreEqual(project.Stories.Length, deserializedProject.Stories.Length);
            for (int i = 0; i < project.Stories.Length; ++i)
            {
                Assert.AreEqual(project.Stories[i].Name, deserializedProject.Stories[i].Name);
                Assert.AreEqual(project.Stories[i].Abbreviation, deserializedProject.Stories[i].Abbreviation);
                Assert.AreEqual(project.Stories[i].Importance, deserializedProject.Stories[i].Importance);
                Assert.AreEqual(project.Stories[i].Estimate, deserializedProject.Stories[i].Estimate);
                Assert.AreEqual(project.Stories[i].StartTime, deserializedProject.Stories[i].StartTime);
                Assert.AreEqual(project.Stories[i].Duration, deserializedProject.Stories[i].Duration);
                Assert.AreEqual(project.Stories[i].HowToDemo, deserializedProject.Stories[i].HowToDemo);
            }
        }

        [Test]
        public void StoryIsDeSerializable()
        {
            var serializer = new XmlSerializer(typeof (Story));
            var deserializedStory = (Story) serializer.Deserialize(new StringReader(expectedStory));
            Assert.AreEqual("FT-1", deserializedStory.Abbreviation);
            Assert.AreEqual("Serialize Story", deserializedStory.Name);
            Assert.AreEqual(-1, deserializedStory.Importance);
            Assert.AreEqual(3, deserializedStory.Estimate);
            Assert.AreEqual("Story ends up in project xml file", deserializedStory.HowToDemo);
            Assert.AreEqual(new DateTime(2008, 4, 19), deserializedStory.Created);
        }

        [Test]
        public void StoryIsSerializable()
        {
            var story = new Story("Serialize Story", "FT-1", new DateTime(2008, 4, 19))
                            {
                                Importance = 42,
                                Estimate = 3,
                                HowToDemo = "Story ends up in project xml file"
                            };

            story.Start(new DateTime(2008, 1, 23));
            story.Pause(new DateTime(2008, 1, 25));
            story.Reset(new DateTime(2008, 4, 19));
            story.Start(new DateTime(2008, 4, 19));
            story.MarkAsDone(new DateTime(2008, 4, 24));

            var serializer = new XmlSerializer(typeof (Story));
            var writer = new StringWriter();
            serializer.Serialize(writer, story);
            XmlAssert.AreEqual(expectedStory, writer.GetStringBuilder().ToString());
        }
    }
}