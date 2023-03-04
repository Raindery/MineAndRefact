using System.Collections;
using UnityEngine;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class Source : MonoBehaviour, ISource
    {
        [SerializeField] private SourceData _sourceSettings;
        [SerializeField] private Transform _resourceDropPoint;


        private int _kickAmountUntilDepletion;


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

        
        private void OnValidate()
        {
            if (_sourceSettings == null)
                throw new System.ArgumentNullException(nameof(_sourceSettings));

            CachedSphereCollider.radius = _sourceSettings.InteractionRadius;
        }

        private void Awake()
        {
            _kickAmountUntilDepletion = _sourceSettings.KickAmountUntilDeplection;
        }


        public void Mine()
        {
            if(_kickAmountUntilDepletion > 0)
            {
                _kickAmountUntilDepletion--;

                if (IsDepletion)
                    StartCoroutine(Recovery());

                DropResources();
            }
        }

        private IEnumerator Recovery()
        {
            yield return new WaitForSecondsRealtime(_sourceSettings.RecoveryDuration);

            _kickAmountUntilDepletion = _sourceSettings.KickAmountUntilDeplection;

            Debug.Log("Source recover!");
            yield break;
        }

        private void DropResources()
        {
            if (_sourceSettings.MiningResource == null)
                throw new System.ArgumentNullException(nameof(_sourceSettings.MiningResource));

            for(int i = 0; i < _sourceSettings.MiningResourceAmount; i++)
            {
                IResource resource = Instantiate(_sourceSettings.MiningResource, _resourceDropPoint.position, Quaternion.identity);
                resource.Drop();
            }

        }
    }
}
