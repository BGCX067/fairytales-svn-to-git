using System;

namespace ConeFabric.FairyTales.Core
{
    public class StoryNotStarted : StoryStatusChange
    {

        public StoryNotStarted(DateTime date) : base(date)
        {}

        public StoryNotStarted() : base(DateTime.Now)
        {}

        public override string Name
        {
            get { return "Not Started"; }
        }
    }
}