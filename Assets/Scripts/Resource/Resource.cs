using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class Resource : MonoBehaviour, IResource
    {
        [SerializeField] private ResourceData _resourceSettings;

        private bool _canPickUp;
        private BoxCollider _collider;
        private bool _hasCollider;
        private Vector3 _minDropImpulse;
        private Vector3 _maxDropImpulse;
        
        public ResourceData ResourceSettings => _resourceSettings;
        public bool CanPickUp => _canPickUp;
        public string ResourceId
        {
            get
            {
                if (_resourceSettings == null)
                    return string.Empty;
                return _resourceSettings.ResourceId;
            }
        }

        private Transform _cachedTransform;
        public Transform CachedTransform
        {
            get
            {
                if (_cachedTransform == null)
                    _cachedTransform = transform;
                return _cachedTransform;
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
            _hasCollider = TryGetComponent<BoxCollider>(out _collider);
        }

        private void OnDisable()
        {
            CachedTransform.DOKill();
        }


        private IEnumerator WaitPickUpDuration()
        {
            _canPickUp = false;

            if(_resourceSettings.PickUpDuration > 0)
                yield return new WaitForSeconds(_resourceSettings.PickUpDuration);

            _canPickUp = true;
            yield break;
        }

        private Vector3 GetRandomImpulse()
        {
            return new Vector3(
                    Random.Range(_minDropImpulse.x, _maxDropImpulse.x),
                    Random.Range(_minDropImpulse.y, _maxDropImpulse.y),
                    Random.Range(_minDropImpulse.z, _maxDropImpulse.z)
                    );
        }

        public Coroutine MoveTo(Vector3 target, float duration)
        {
            return StartCoroutine(MoveToCoroutine(target, duration));
        }
        private IEnumerator MoveToCoroutine(Vector3 target, float duration)
        {
            yield return CachedTransform.DOMove(target, duration).WaitForCompletion();
            yield break;
        }

        public void Extract()
        {
            StartCoroutine(WaitPickUpDuration());
            Vector3 impulseVector = GetRandomImpulse();
            CachedRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }

        public void PickUp()
        {
            ResourceDestroy();
        }

        public void SetEnableInteractionComponents(bool value)
        {
            if (_hasCollider)
                _collider.enabled = value;
            CachedRigidbody.isKinematic = !value;
            CachedSphereCollider.enabled = !value;
        }

        public void ResourceDestroy()
        {
            Destroy(gameObject);
        }
    }
}