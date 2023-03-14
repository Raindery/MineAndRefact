using DG.Tweening;
using MineAndRefact.Core.UI;
using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class BaseSpot : MonoBehaviour, ISpot
    {
        protected const float RECYCLING_PROGRESS_UPDATE_STEP = 0.1f;

        [Header("General")]
        [SerializeField] private SpotData _spotSettings;
        [SerializeField] private Transform _resourceDropPoint;
        [Header("UI")]
        [SerializeField] private UISpot _uiSpot;
        [Header("Model")]
        [SerializeField] private Transform _spotModel;
        [Header("Animation")]
        [SerializeField] private Vector3 _additionalScaleOnDrop = Vector3.one;
        [SerializeField] private float _scaleOnDropDuration = 0.5f;

        private int _remainsToLoadAmountResources;
        private bool _isRecyclingProcessed;

        protected Vector3 DropResourcePosition
        {
            get
            {
                if (_resourceDropPoint != null)
                    return _resourceDropPoint.position;

                return CachedTransform.position;
            }
        }
        protected bool HasSpotUi => _uiSpot != null;
        protected bool HasModel => _spotModel != null;
        protected UISpot UISpot => _uiSpot;
        protected Transform SpotModel => _spotModel;

        public SpotData SpotSettings => _spotSettings;
        public bool IsFullLoaded => _remainsToLoadAmountResources <= 0;
        public bool IsRecyclingProcessed => _isRecyclingProcessed;
        public int RemainsToLoadAmountResources => _remainsToLoadAmountResources;


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
        }

        private void Start()
        {
            if (HasSpotUi)
            {
                _uiSpot.ShowAmountRequired();
                _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);
            }
        }

        private void OnDisable()
        {
            if(HasModel)
                _spotModel.DOKill();
        }


        private IEnumerator RecyclingCoroutine()
        {
            _isRecyclingProcessed = true;
            float recyclingDuration = _spotSettings.RecyclingDuration;

            if (HasSpotUi)
                _uiSpot.ShowRecyclingProgress();

            while (recyclingDuration > 0)
            {
                if (HasSpotUi)
                    _uiSpot.UpdateRecyclingProgress(recyclingDuration);
                yield return new WaitForSeconds(RECYCLING_PROGRESS_UPDATE_STEP);
                recyclingDuration -= RECYCLING_PROGRESS_UPDATE_STEP;
            }

            if (HasSpotUi)
                _uiSpot.HideRecyclingProgress();

            if (_spotSettings.ReceivedResource == null)
                throw new System.ArgumentNullException(nameof(_spotSettings.ReceivedResource));

            for (int i = 0; i < _spotSettings.AmountReceivedResource; i++)
            {
                IResource resource = Instantiate(_spotSettings.ReceivedResource, DropResourcePosition + _spotSettings.RandomDropResourceOffset, Quaternion.identity);
                resource.Extract();
            }

            _remainsToLoadAmountResources = _spotSettings.AmountRequiredResource;
            if (HasSpotUi)
            {
                _uiSpot.ShowAmountRequired();
                _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);
            }

            _isRecyclingProcessed = false;
            yield break;
        }
        public Coroutine Recycling()
        {
            return StartCoroutine(RecyclingCoroutine());
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

                if (HasSpotUi)
                    _uiSpot.UpdateAmountResource(_remainsToLoadAmountResources);


                if (HasModel)
                {
                    _spotModel.DOComplete();
                    _spotModel.DOPunchScale(_additionalScaleOnDrop, _scaleOnDropDuration, elasticity: 0.5f);
                }

                if (IsFullLoaded)
                {
                    if (HasSpotUi)
                        _uiSpot.HideAmountRequired();

                    Recycling();
                }
            }
        }
    }
}