namespace MineAndRefact.Core
{
    public interface ISpot
    {
        SpotData SpotSettings { get; }
        UnityEngine.Transform CachedTransform { get; }
        bool IsFullLoaded { get; }
        bool IsRecyclingProcessed { get; }
        int RemainsToLoadAmountResources { get; }

        void LoadRequiredResource(string resourceId);
    }
}