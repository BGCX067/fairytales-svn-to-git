using System.IO;
using System.Xml.Serialization;
using System;
using System.Xml;

namespace ConeFabric.FairyTales.Core
{

    public class InvalidProjectStreamException : Exception
    {
    }
    public class ProjectFile
    {
        public static Project Load(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Project));

            if (IsValidStream(stream, serializer))
            {
                using (stream)
                {
                    var project = serializer.Deserialize(stream) as Project;
                    return project;
                }
            }
            throw new InvalidProjectStreamException();
        }

        public static void Create(string fileName, Project project)
        {
            var file = new FileInfo(fileName);
            if (file.Exists)
                Write(file.Open(FileMode.Truncate), project);
            else
                Write(file.OpenWrite(), project);
        }

        public static string CreateHomeDirectory(string persistanceDirectory, Project project)
        {
            if (persistanceDirectory == string.Empty)
                throw new DirectoryNotFoundException("Can't create the root directory <String.Empty>");
            if (project == null || project.Name == string.Empty)
                throw new ArgumentException("Either project is null, or it doesn't have a name that can be a directory");

            var projectOutputDirectory = string.Format("{0}/{1}", persistanceDirectory, project.Name);
            var directory = new DirectoryInfo(projectOutputDirectory);
            if (!directory.Exists)
                directory.Create();
            return projectOutputDirectory;
        }

        private static bool IsValidStream(Stream stream, XmlSerializer serializer)
        {
            try
            {
                var reader = new XmlTextReader(stream);
                if (!serializer.CanDeserialize(reader))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            stream.Seek(0, SeekOrigin.Begin);
            return true;
        }

        private static void Write(Stream stream, Project project)
        {
            var serializer = new XmlSerializer(typeof(Project));
            var writer = new StreamWriter(stream);
            serializer.Serialize(writer, project);
            stream.Close();
        }
    }
}
