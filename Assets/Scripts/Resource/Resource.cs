using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class Resource : MonoBehaviour, IResource
    {
        [SerializeField] private ResourceData _resourceSettings;


        private bool _canPickUp;
        private Vector3 _minDropImpulse;
        private Vector3 _maxDropImpulse;


        public ResourceData ResourceSettings => _resourceSettings;
        public bool CanPickUp => _canPickUp;
        public ResourceType Type
        {
            get
            {
                if (_resourceSettings == null)
                    return ResourceType.Default;
                return _resourceSettings.ResourceType;
            }
        }


        private Rigidbody _rigidbody;
        public Rigidbody CachedRigidbody
        {
            get
            {
                if (_rigidbody == null)
                    _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        private SphereCollider _cachedSphereCollider;
        public SphereCollider CachedSphereCollider
        {
            get
            {
                if (_cachedSphereCollider == null)
                    _cachedSphereCollider = GetComponent<SphereCollider>();
                return _cachedSphereCollider;
            }
        }


        private void OnValidate()
        {
            if (_resourceSettings == null)
                throw new System.ArgumentNullException(nameof(_resourceSettings));

            CachedSphereCollider.radius = _resourceSettings.PickUpRadius;
        }

        private void Awake()
        {
            if (_resourceSettings == null)
                throw new System.ArgumentNullException(nameof(_resourceSettings));

            CachedSphereCollider.radius = _resourceSettings.PickUpRadius;

            _minDropImpulse = _resourceSettings.MinDropImpulse;
            _maxDropImpulse = _resourceSettings.MaxDropImpulse;
        }


        private IEnumerator WaitPickUpDuration()
        {
            _canPickUp = false;

            if(_resourceSettings.PickUpDuration > 0)
                yield return new WaitForSeconds(_resourceSettings.PickUpDuration);

            _canPickUp = true;

            yield break;
        }

        public void Drop()
        {
            StartCoroutine(WaitPickUpDuration());

            Vector3 impulseVector = new Vector3(
                    Random.Range(_minDropImpulse.x, _maxDropImpulse.x),
                    Random.Range(_minDropImpulse.y, _maxDropImpulse.y),
                    Random.Range(_minDropImpulse.z, _maxDropImpulse.z)
                    );

            CachedRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }

        public void PickUp()
        {
            Destroy(gameObject);
        }
    }

    public enum ResourceType
    {
        Default,
        Wood,
        Metal,
        Crystal
    }
}