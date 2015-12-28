using System;

namespace ConeFabric.FairyTales.Core
{
    public class ProjectStartsWithDotException : ArgumentException
    {
        public override string Message
        {
            get { return "Fairy Tales does not support project names that starts with a dot (.), please choose another."; }
        }
    }
}