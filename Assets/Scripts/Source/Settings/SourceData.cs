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
        [SerializeField] private BaseResource _miningResource;

        public float InteractionRadius => _interactionRadius;
        public float MineSpeed => _mineSpeed;
        public int MiningResourceAmount => _miningResourcesAmount;
        public int KickAmountUntilDeplection => _kickAmountUntilDepletion;
        public float RecoveryDuration => _recoveryDuration;
        public BaseResource MiningResource => _miningResource;
    }
}