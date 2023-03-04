using UnityEngine;

namespace MineAndRefact.Core
{
    [CreateAssetMenu(fileName = "ResourceSettings", menuName = "Mine And Refact/Create Resource Settings")]
    public class ResourceData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private float _pickUpDuration;
        [Min(0.1f)]
        [SerializeField] private float _pickUpRadius;
        [Header("Impulse")]
        [SerializeField] private Vector3 _minDropImpulse;
        [SerializeField] private Vector3 _maxDropImpulse;


        public float PickUpDuration => _pickUpDuration;
        public float PickUpRadius => _pickUpRadius;
        public Vector3 MinDropImpulse => _minDropImpulse;
        public Vector3 MaxDropImpulse => _maxDropImpulse;


        private void OnValidate()
        {
            if (_minDropImpulse.x > _maxDropImpulse.x)
                _minDropImpulse.x = _maxDropImpulse.x;

            if (_minDropImpulse.y > _maxDropImpulse.y)
                _minDropImpulse.y = _maxDropImpulse.y;

            if (_minDropImpulse.z > _maxDropImpulse.z)
                _minDropImpulse.z = _maxDropImpulse.z;
        }
    }
}