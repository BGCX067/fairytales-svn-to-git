using System;

namespace ConeFabric.FairyTales.Core
{
    public class StoryDone : StoryStatusChange
    {
        public StoryDone(DateTime date, int importance): base(date)
        {
            Importance = importance;
        }

        public override string Name
        {
            get { return "Done"; }
        }
        public int Importance { get; set; }

        public StoryDone()
        {}
    }
}