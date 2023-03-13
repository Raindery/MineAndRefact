using UnityEngine;

namespace MineAndRefact.Core
{
    [CreateAssetMenu(fileName = "SourceSettings", menuName = "Mine And Refact/Create Spot Settings Asset")]
    public class SpotData : ScriptableObject
    {
        [Header("General")]
        [Min(0.1f)]
        [SerializeField] private float _iteractionRadius;
        [Header("Required Resource Settings")]
        [SerializeField] private BaseResource _requiredResource;
        [Min(1)]
        [SerializeField] private int _amountRequiredResource;
        [Header("Received Resource Settingss")]
        [SerializeField] private BaseResource _receivedResource;
        [Min(1)]
        [SerializeField] private int _amountReceivedResource;
        [Header("Recycling")]
        [SerializeField] private float _recyclingDuration = 5f;

        public float IteractionRadius => _iteractionRadius;
        public int AmountRequiredResource => _amountRequiredResource;
        public BaseResource RequiredResource => _requiredResource;
        public BaseResource ReceivedResource => _receivedResource; 
        public int AmountReceivedResource => _amountReceivedResource;
        public float RecyclingDuration => _recyclingDuration;
    }
}