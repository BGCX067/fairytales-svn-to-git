using System;

namespace ConeFabric.FairyTales.Core
{
    public class StoryPaused : StoryStatusChange
    {
        public StoryPaused(DateTime date): base(date)
        {}
        public override string Name
        {
            get { return "Paused"; }
        }
        public StoryPaused()
        {}
        
    }
}