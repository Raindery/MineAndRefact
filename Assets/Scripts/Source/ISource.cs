namespace MineAndRefact.Core
{
    public interface ISource
    {
        SourceData SourceSettings { get; }
        bool IsDepletion { get; }

        void Mine();
    }
}