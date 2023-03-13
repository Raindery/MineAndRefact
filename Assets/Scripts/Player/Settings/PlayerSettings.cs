using UnityEngine;

namespace MineAndRefact.Core
{
    [CreateAssetMenu(fileName = "PlayerCharacterSettings", menuName = "Mine And Refact/Create Player Character Settings Asset")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("General")]
        [Min(0.5f)]
        [SerializeField] private float _movementMultiplier = 1f;
        [Min(0.01f)]
        [SerializeField] private float _defaultActionDelay = 1f;
        [Header("Resource Scutter On Drop In Spot")]
        [Min(1)]
        [SerializeField] private int _maxDropResourceAmountInOneTime = 2;
        [SerializeField] private Vector3 _minDropResourceInSpotScatter;
        [SerializeField] private Vector3 _maxDropResourceInSpotScatter;
        [SerializeField] private float _scutterMovementDuration = 0.5f;
        [SerializeField] private float _dropResourceInSpotDelay = 0.2f;
        [SerializeField] private float _dropResourceInSpotDuration = 0.5f;

        public int MaxDropResourceAmountInOneTime => _maxDropResourceAmountInOneTime;
        public float MovementMultiplier => _movementMultiplier;
        public float DefaultActionDelay => _defaultActionDelay;
        public Vector3 MinDropResourceInSpotScatter => _minDropResourceInSpotScatter;
        public Vector3 MaxDropResourceInSpotScatter => _maxDropResourceInSpotScatter;
        public float ScutterMovementDuration => _scutterMovementDuration;
        public float DropResourceInSpotDelay => _dropResourceInSpotDelay;
        public float DropResourceInSpotDuration => _dropResourceInSpotDuration;


        private void OnValidate()
        {
            if (_minDropResourceInSpotScatter.x > _maxDropResourceInSpotScatter.x)
                _maxDropResourceInSpotScatter.x = _minDropResourceInSpotScatter.x;

            if (_minDropResourceInSpotScatter.y > _maxDropResourceInSpotScatter.y)
                _maxDropResourceInSpotScatter.y = _minDropResourceInSpotScatter.y;

            if (_minDropResourceInSpotScatter.z > _maxDropResourceInSpotScatter.z)
                _maxDropResourceInSpotScatter.z = _minDropResourceInSpotScatter.z;
        }
    }
}