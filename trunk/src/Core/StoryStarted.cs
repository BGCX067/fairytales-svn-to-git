using System;

namespace ConeFabric.FairyTales.Core
{
    public class StoryStarted : StoryStatusChange
    {
        public StoryStarted(DateTime date): base(date)
        {}
        public override string Name
        {
            get { return "Started"; }
        }
        public StoryStarted()
        {}
    }
}