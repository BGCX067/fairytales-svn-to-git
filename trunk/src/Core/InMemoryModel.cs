using System;

namespace ConeFabric.FairyTales.Core
{
    public class InMemoryModel : ProjectModel
    {
        public override bool SaveActiveProject()
        {
            return false;
        }
        public override void Load()
        {
            AddProject("Fairy Tales");
            AddStory("FT", "Not Started");
            AddStory("FT", "Started");
            AddStory("FT", "Paused");
            AddStory("FT", "Done");

            var active = GetActiveProject();

            active.Stories[0].Importance = 50;

            active.Stories[1].Importance = 200;
            active.Stories[1].Start(DateTime.Now);
            
            active.Stories[2].Importance = 100;
            active.Stories[2].Pause();
            
            active.Stories[3].Importance = 300;
            active.Stories[3].MarkAsDone(DateTime.Now);
            
        }
    }
}
