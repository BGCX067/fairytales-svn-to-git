namespace ConeFabric.FairyTales.Core
{
    public interface IStoryFilter
    {
        bool Include(Story story);
        bool IsActive { get; }
    }
}