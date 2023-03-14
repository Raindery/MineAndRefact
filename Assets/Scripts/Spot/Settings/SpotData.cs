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
        [Min(0)]
        [SerializeField] private float _recyclingDuration = 5f;
        [SerializeField] private Vector3 _minDropResourceOffset = -Vector3.one;
        [SerializeField] private Vector3 _maxDropResourceOffset = Vector3.one;

        public float IteractionRadius => _iteractionRadius;
        public int AmountRequiredResource => _amountRequiredResource;
        public BaseResource RequiredResource => _requiredResource;
        public BaseResource ReceivedResource => _receivedResource; 
        public int AmountReceivedResource => _amountReceivedResource;
        public float RecyclingDuration => _recyclingDuration;
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