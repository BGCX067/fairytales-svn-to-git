using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConeFabric.FairyTales.Core
{
    public class Story : DomainObject
    {
        public Story(string name, string abbreviation)
            : this(name, abbreviation, DateTime.Now)
        { }
        public Story(string name, string abbreviation, DateTime creationTime)
            : base(name, abbreviation)
        {
            StartTime = DateTime.MinValue;
            Created = creationTime;
        }

        internal Story(Project owner, string name, string abbreviation)
            : this(name, abbreviation)
        {
            this.owner = owner;
        }

        [XmlElement(Order = 1)]
        public int Importance
        {
            get { return importance; }
            set
            {
                HandleCollisions(value);
                importance = value;
            }
        }

        [XmlElement(Order = 2)]
        public int Estimate { get; set; }

        [XmlElement(Order = 3)]
        public StoryStatusChange ActiveStatus
        {
            get
            {
                if (statusHistory.Count == 0)
                    return new StoryNotStarted();
                return statusHistory[statusHistory.Count - 1];
            }
        }

        [XmlElement(Order = 4)]
        public DateTime StartTime { get; set; }

        [XmlElement(Order = 5)]
        public DateTime Created { get; set; }

        [XmlElement(Order = 6)]
        public string HowToDemo { get; set; }

        [XmlArray(Order = 7)]
        public StoryStatusChange[] StatusChange
        {
            get { return statusHistory.ToArray(); }
            set
            {
                statusHistory.Clear();
                statusHistory.AddRange(value);
            }
        }

        public void Start(DateTime startTime)
        {
            if (IsStarted)
                return;
            statusHistory.Add(new StoryStarted(startTime));
            StartTime = startTime;
            Importance = GetLastImportance();
        }

        public bool IsDone { get { return ActiveStatus is StoryDone;} }
        public bool IsPaused { get { return ActiveStatus is StoryPaused; } }
        public bool IsStarted { get { return ActiveStatus is StoryStarted; } }
        
        public void Pause()
        {
            if (IsPaused)
                return;
            Pause(DateTime.Now);
        }

        public void Pause(DateTime pauseDate)
        {
            statusHistory.Add(new StoryPaused(pauseDate));
        }
        
        public void Reset()
        {
            Reset(DateTime.Now);
        }
        public void Reset(DateTime date)
        {
            if (IsStarted || IsPaused || IsDone)
                statusHistory.Add(new StoryNotStarted(date));
        }

        public TimeSpan Duration
        {
            get { return duration; }
        }

        public void MarkAsDone(DateTime endDate)
        {
            if (IsDone)
                return;
            if (StartTime == DateTime.MinValue)
                StartTime = endDate;
            duration = endDate - StartTime;
            statusHistory.Add(new StoryDone(endDate, Importance));
            Importance = -1;
        }

        [XmlIgnore]
        public Project Owner
        {
            get { return owner; }
            internal set { owner = value; }
        }

        private void HandleCollisions(int value)
        {
            if (value <= 0 || owner == null)
                return;

            Story alreadyThere;
            if (!owner.TryGetStoryByImportance(value, out alreadyThere))
                return;
            alreadyThere.Importance++;         
        }

        private int GetLastImportance()
        {
            for (int i = statusHistory.Count - 1; i >= 0; --i)
            {
                var done = statusHistory[i] as StoryDone;
                if (done != null)
                    return done.Importance;
            }
            return Importance;
        }

        private readonly List<StoryStatusChange> statusHistory = new List<StoryStatusChange>();
        private TimeSpan duration = TimeSpan.Zero;
        private int importance;
        private Project owner;
        private Story() : this("Untitled Story", "UP-X") { }
    }
}
