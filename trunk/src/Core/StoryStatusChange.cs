using System;
using System.Xml.Serialization;

namespace ConeFabric.FairyTales.Core
{
    [XmlInclude(typeof(StoryStarted)),
    XmlInclude(typeof(StoryPaused)),
    XmlInclude(typeof(StoryNotStarted)),
    XmlInclude(typeof(StoryDone))]
    public abstract class StoryStatusChange
    {
        protected StoryStatusChange()
        { }

        protected StoryStatusChange(DateTime date)
        {
            Date = date;
        }
        public abstract string Name { get; }
        public DateTime Date
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}