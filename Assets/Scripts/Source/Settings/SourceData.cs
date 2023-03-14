using UnityEngine;

namespace MineAndRefact.Core
{
    [CreateAssetMenu(fileName = "SourceSettings", menuName = "Mine And Refact/Create Source Settings")]
    public class SourceData : ScriptableObject
    {
        [Header("General")]
        [Min(1f)]
        [SerializeField] private float _interactionRadius = 1f;
        [Min(0.05f)]
        [SerializeField] private float _mineSpeed = 0.05f;
        [Min(1)]
        [SerializeField] private int _miningResourcesAmount = 1;
        [Min(1)]
        [SerializeField] private int _kickAmountUntilDepletion = 1;
        [Min(1f)]
        [SerializeField] private float _recoveryDuration = 1f;
        [Space]
        [Header("Mining Resource")]
        [SerializeField] private BaseResource _miningResource;
        [SerializeField] private Vector3 _minDropResourceOffset = -Vector3.one;
        [SerializeField] private Vector3 _maxDropResourceOffset = Vector3.one;

        public float InteractionRadius => _interactionRadius;
        public float MineSpeed => _mineSpeed;
        public int MiningResourceAmount => _miningResourcesAmount;
        public int KickAmountUntilDeplection => _kickAmountUntilDepletion;
        public float RecoveryDuration => _recoveryDuration;
        public BaseResource MiningResource => _miningResource;
        public Vector3 MinDropResourceOffset => _minDropResourceOffset;
        public Vector3 MaxDropResourceOffset => _maxDropResourceOffset;
        public Vector3 RandomDropResourceOffset
        {
            get => new Vector3(
                Random.Range(_minDropResourceOffset.x, _maxDropResourceOffset.x),
                Random.Range(_minDropResourceOffset.y, _maxDropResourceOffset.y),
                Random.Range(_minDropResourceOffset.z, _maxDropResourceOffset.z)
                );
        }


        private void OnValidate()
        {
            if (_minDropResourceOffset.x > _maxDropResourceOffset.x)
                _minDropResourceOffset.x = _maxDropResourceOffset.x;

            if (_minDropResourceOffset.y > _maxDropResourceOffset.y)
                _minDropResourceOffset.y = _maxDropResourceOffset.y;

            if (_minDropResourceOffset.z > _maxDropResourceOffset.z)
                _minDropResourceOffset.z = _maxDropResourceOffset.z;
        }
    }
}