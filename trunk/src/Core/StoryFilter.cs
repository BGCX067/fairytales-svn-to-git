using System;
using System.Collections.Generic;

namespace ConeFabric.FairyTales.Core
{
    public class StoryFilter : IStoryFilter
    {
        public bool IsActive { get { return exclude.Count != 0;} }

        public void AddStoryFilter(Type type)
        {
            exclude.Add(type);
        }

        public void AddStoryFilter<TStatus>() where TStatus : StoryStatusChange
        {
            AddStoryFilter(typeof(TStatus));
        }

        public void RemoveStoryFilter(Type type)
        {
            exclude.Remove(type);
        }

        public void RemoveStoryFilter<TStatus>() where TStatus : StoryStatusChange
        {
            RemoveStoryFilter(typeof(TStatus));
        }

        public bool Include(Story story)
        {
            return !IsActive || !Exclude(story);
        }

        private bool Exclude(Story story)
        {
            foreach(var status in exclude)
                if(status.IsAssignableFrom(story.ActiveStatus.GetType()))
                    return true;
            return false;
        }

        private readonly List<Type> exclude = new List<Type>();
    }
}