using DG.Tweening;
using MineAndRefact.Core.UI;
using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class BaseSource : MonoBehaviour, ISource
    {
        private const float RECOVERY_UPDATE_STEP = 0.1f;

        [Header("General")]
        [SerializeField] private SourceData _sourceSettings;
        [SerializeField] private Transform _resourceDropPoint;
        [Header("UI")]
        [SerializeField] private UISource _uiSource;
        [Header("Animation Settings")]
        [SerializeField] private Vector3 _scaleOnMining;
        [SerializeField] private float _scaleOnMiningDuration;

        private bool _hasUi;
        private int _kickAmountUntilDepletion;

        private Vector3 DropResourcePosition
        {
            get
            {
                if (_resourceDropPoint != null)
                    return _resourceDropPoint.position;
                return CachedTransform.position;
            }
        }
        public bool IsDepletion => _kickAmountUntilDepletion <= 0;
        public SourceData SourceSettings => _sourceSettings;

        private SphereCollider _sphereCollider;
        public SphereCollider CachedSphereCollider
        {
            get
            {
                if (_sphereCollider == null)
                    _sphereCollider = GetComponent<SphereCollider>();
                return _sphereCollider;
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

        
        private void OnValidate()
        {
            if (_sourceSettings == null)
                throw new System.ArgumentNullException(nameof(_sourceSettings));

            CachedSphereCollider.radius = _sourceSettings.InteractionRadius;
        }

        private void Awake()
        {
            if (_sourceSettings == null)
                throw new System.ArgumentNullException(nameof(_sourceSettings));

            CachedSphereCollider.radius = _sourceSettings.InteractionRadius;
            _kickAmountUntilDepletion = _sourceSettings.KickAmountUntilDeplection;
            _hasUi = _uiSource != null;
        }

        private void OnDisable()
        {
            CachedTransform.DOKill();
        }

        private IEnumerator Recovery()
        {
            float recoveryDuration = _sourceSettings.RecoveryDuration;

            if (_hasUi)
                _uiSource.ShowRecoveryDuration();

            while (recoveryDuration > 0f)
            {
                if (_hasUi)
                    _uiSource.DisplayRecoveryDuration(recoveryDuration);

                yield return new WaitForSeconds(RECOVERY_UPDATE_STEP);
                recoveryDuration -= RECOVERY_UPDATE_STEP;
            }

            if (_hasUi)
                _uiSource.HideRecoveryDuration();

            _kickAmountUntilDepletion = _sourceSettings.KickAmountUntilDeplection;
            Debug.Log("Source recover!");
            yield break;
        }

        private void ExtractResources()
        {
            if (_sourceSettings.MiningResource == null)
                throw new System.ArgumentNullException(nameof(_sourceSettings.MiningResource));

            for (int i = 0; i < _sourceSettings.MiningResourceAmount; i++)
            {
                IResource resource = Instantiate(_sourceSettings.MiningResource, DropResourcePosition, Quaternion.identity);
                resource.Extract();
            }

        }

        public void Mine()
        {
            if(_kickAmountUntilDepletion > 0)
            {
                _kickAmountUntilDepletion--;

                if (IsDepletion)
                    StartCoroutine(Recovery());

                CachedTransform.DOComplete();
                CachedTransform.DOPunchScale(_scaleOnMining, _scaleOnMiningDuration, elasticity: 0.5f);

                ExtractResources();
            }
        }
    }
}