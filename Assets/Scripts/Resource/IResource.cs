using UnityEngine;

namespace MineAndRefact.Core
{
    public interface IResource
    {
        ResourceData ResourceSettings { get; }
        bool CanPickUp { get; }
        string ResourceId { get; }
        UnityEngine.Transform CachedTransform { get; }
        UnityEngine.SphereCollider CachedSphereCollider { get; }

        void Extract();
        void PickUp();
        Coroutine MoveTo(Vector3 target, float duration);
        void SetEnableInteractionComponents(bool value);
        void ResourceDestroy();
    }
}