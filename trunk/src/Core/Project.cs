using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConeFabric.FairyTales.Core
{
    public class Project : DomainObject
    {
        public Project(string name, string abbreviation)
            : base(name, abbreviation)
        { }

        public void Update(Story story)
        {
            Story tmp;
            if (!storiesByAbbrivation.TryGetValue(story.Abbreviation, out tmp))
                throw new ArgumentException(string.Format("Uppdate Error: Invalid Story abbrivation ({0}) in project: {1}", story.Abbreviation, Name));

        }
        public void Delete(Story story)
        {
            Delete(story.Abbreviation);
        }

        public void Delete(string abbreviation)
        {
            if (!storiesByAbbrivation.Remove(abbreviation))
                throw new ArgumentException(string.Format("Delete Error: Invalid Story abbrivation ({0}) in project: {1}", abbreviation, Name));
        }

        [XmlArray(Order = 1)]
        public Story[] Stories
        {
            get { return ListStories(); }
            set 
            {
                storyCounter = StoryOrderHelper.FindHighestNumber(value);

                foreach (Story story in value)
                    AddStory(story);                
            }
        }

        public Story[] ListStoriesByImportance(bool ascending)
        {
            var stories = ListStories();
            StoryOrderHelper.Sort(stories, ascending);
            return stories;
        }

        public void AddStory(string storyTitle)
        {
            AddStory(new Story(this, storyTitle, NextNumber()));
        }

        private Story[] ListStories()
        {
            var stories = new List<Story>();
            foreach (var story in storiesByAbbrivation.Keys)
                stories.Add(storiesByAbbrivation[story]);
            return stories.ToArray();
        }

        private void AddStory(Story story)
        {
            story.Owner = this;
            storiesByAbbrivation.Add(story.Abbreviation, story);
        }

        private string NextNumber()
        {
            return string.Format("{0}-{1}", Abbreviation, ++storyCounter);
        }

        internal bool TryGetStoryByImportance(int importance, out Story value)
        {
            foreach(var item in storiesByAbbrivation)
                if(item.Value.Importance == importance)
                {
                    value = item.Value;
                    return true;
                }
            value = null;
            return false;
        }

        private static class StoryOrderHelper
        {
            public static int FindHighestNumber(ICollection<Story> value)
            {
                var highest = value.Count;
                foreach (var story in value)
                {
                    var abbreviation = story.Abbreviation.Split(new[] { '-' });
                    var number = Convert.ToInt32(abbreviation[1]);
                    if (number > highest)
                        highest = number;
                }
                return highest;
            }

            public static void Sort(Story[] stories, bool ascending)
            {
                var returnvalue = ascending ? -1 : 1;
                Array.Sort(stories, delegate(Story first, Story other)
                {
                    if (first.Importance < other.Importance)
                        return returnvalue;
                    else if (first.Importance == other.Importance)
                        return 0;
                    return -returnvalue;
                });
            }
        }


        // Needed for serializing
        private Project() : base("Undefined Project", "UP") { }

        private int storyCounter;
        private readonly Dictionary<string, Story> storiesByAbbrivation = new Dictionary<string, Story>();
    }
}
