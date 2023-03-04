namespace MineAndRefact.Core
{
    public interface IResource
    {
        ResourceData ResourceSettings { get; }
        bool CanPickUp { get; }

        void Drop();
        void PickUp();
    }
}