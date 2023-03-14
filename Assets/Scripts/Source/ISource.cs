namespace MineAndRefact.Core
{
    public interface ISource
    {
        SourceData SourceSettings { get; }
        bool IsDepletion { get; }
        UnityEngine.Transform CachedTransform { get; }

        void Mine();
    }
}