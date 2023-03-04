namespace MineAndRefact.Core
{
    public interface IResource
    {
        ResourceData ResourceSettings { get; }
        bool CanPickUp { get; }
        ResourceType Type { get; }

        void Drop();
        void PickUp();
    }
}