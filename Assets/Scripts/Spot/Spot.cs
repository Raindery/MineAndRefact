using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineAndRefact.Core.UI;
using DG.Tweening;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class Spot : MonoBehaviour, ISpot
    {
        private const float RECYCLING_PROGRESS_UPDATE_STEP = 0.1f;

        [Header("General")]
        [SerializeField] private SpotData _spotSettings;
        [SerializeField] private Transform _resourceDropPoint;
        [Header("UI")]
        [SerializeField] private UISpot _uiSpot;
        [Header("Animation")]
        [SerializeField] private Vector3 _additionalScaleOnDrop = Vector3.one;
        [SerializeField] private float _scaleOnDropDuration = 0.5f;

        private int _remainsToLoadAmountResources;
        private bool _isRecyclingProcessed;
        private bool _hasSpotUi;

        public SpotData SpotSettings => _spotSettings;


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

        private Vector3 DropResourcePosition
        {
            get
            {
                if (_resourceDropPoint != null)
                    return _resourceDropPoint.position;

                return CachedTransform.position;
            }
        }

        public bool IsFullLoaded => _remainsToLoadAmountResources <= 0;

        public bool IsRecyclingProcessed => _isRecyclingProcessed;

        public int RemainsToLoadAmountResources => _remainsToLoadAmountResources;

        

        private void OnValidate()
        {
            if (_spotSettings == null)
                throw new System.ArgumentNullException(nameof(_spotSettings));

            CachedSphereCollider.radius = _spotSettings.IteractionRadius;
        }

        private void Awake()
        {
            if (_spotSettings == null)
                throw new System.ArgumentNullException(nameof(_spotSettings));

            CachedSphereCollider.radius = _spotSettings.IteractionRadius;
            _remainsToLoadAmountResources = _spotSettings.AmountRequiredResource;
            _hasSpotUi = _uiSpot != null;
        }

        private void Start()
        {
            if (_hasSpotUi)
            {
                _uiSpot.ShowAmountRequired();
                _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);
            }
        }

        private void OnDisable()
        {
            CachedTransform.DOKill();
        }

        public void LoadRequiredResource(string resourceId)
        {
            if(resourceId != _spotSettings.RequiredResource.ResourceId)
            {
                Debug.LogWarning("Unable load required resource because resource id is wrong!");
                return;
            }

            if(_remainsToLoadAmountResources > 0)
            {
                _remainsToLoadAmountResources--;

                if (_hasSpotUi)
                    _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);

                CachedTransform.DORewind();
                CachedTransform.DOPunchScale(_additionalScaleOnDrop, _scaleOnDropDuration);

                if (IsFullLoaded)
                {
                    if (_hasSpotUi)
                        _uiSpot.HideAmountRequired();
                    
                    StartCoroutine(RecyclingCoroutine());
                }
            }
        }


        private IEnumerator RecyclingCoroutine()
        {
            _isRecyclingProcessed = true;

            float recyclingDuration = _spotSettings.RecyclingDuration;

            if (_hasSpotUi)
                _uiSpot.ShowRecyclingProgress();

            while(recyclingDuration > 0)
            {
                if (_hasSpotUi)
                    _uiSpot.UpdateRecyclingProgress(recyclingDuration);
                yield return new WaitForSeconds(RECYCLING_PROGRESS_UPDATE_STEP);
                recyclingDuration -= RECYCLING_PROGRESS_UPDATE_STEP;
            }

            if (_hasSpotUi)
                _uiSpot.HideRecyclingProgress();

            if (_spotSettings.ReceivedResource == null)
                throw new System.ArgumentNullException(nameof(_spotSettings.ReceivedResource));

            for (int i = 0; i < _spotSettings.AmountReceivedResource; i++)
            {
                IResource resource = Instantiate(_spotSettings.ReceivedResource, DropResourcePosition, Quaternion.identity);
                resource.Extract();
            }

            _remainsToLoadAmountResources = _spotSettings.AmountRequiredResource;
            if (_hasSpotUi)
            {
                _uiSpot.ShowAmountRequired();
                _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);
            }
            _isRecyclingProcessed = false;

            yield break;
        }
    }
}


