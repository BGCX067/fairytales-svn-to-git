using System;

namespace ConeFabric.FairyTales.Core
{
    public class ProjectAllreadyExistsException : ArgumentException
    {
        public ProjectAllreadyExistsException(string projectName)
        {
            name = projectName;
        }
        public override string Message
        {
            get { return string.Format("A project with the name <{0}> allready exists. Please choose another name for your project.", name); }
        }

        readonly string name;
    }
}