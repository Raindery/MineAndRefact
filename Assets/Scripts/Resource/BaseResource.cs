using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class BaseResource : MonoBehaviour, IResource
    {
        [SerializeField] private ResourceData _resourceSettings;

        private bool _canPickUp;
        
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
            Vector3 impulseVector = _resourceSettings.RandomDropImpulse;
            CachedRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }

        public void PickUp()
        {
            ResourceDestroy();
        }

        public void SetEnableInteractionComponents(bool value)
        {
            CachedRigidbody.isKinematic = !value;
            CachedSphereCollider.enabled = !value;
        }

        public void ResourceDestroy()
        {
            Destroy(gameObject);
        }
    }
}