using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MineAndRefact.Core.UI
{
    public class UIResource : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private ResourceData _resourceComporator;
        [SerializeField] private Image _image;
        [SerializeField] private Text _amountText;
        [Header("Animation Settings")]
        [SerializeField] private Vector2 _addingScale;
        [SerializeField] private float _duration;

        private int _resourceAmount;

        private Transform _cahcedTransform;
        public Transform CachedTransform
        {
            get
            {
                if (_cahcedTransform == null)
                    _cahcedTransform = transform;
                return _cahcedTransform;
            }
        }


        private void Awake()
        {
            if (_amountText == null)
                throw new System.ArgumentNullException(nameof(_amountText));

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CachedTransform.DOKill();
        }


        public void ChangeResourceAmount(int amount)
        {
            gameObject.SetActive(amount > 0);
            _resourceAmount = amount;
            _amountText.text = _resourceAmount.ToString();
        }

        public void OnResourceAmountChanged(string resourceId, int changedAmount)
        {
            if (_resourceComporator.ResourceId != resourceId)
                return;

            if (_resourceAmount < changedAmount)
            {
                CachedTransform.DOComplete();
                CachedTransform.DOPunchScale(_addingScale, _duration);
            }

            ChangeResourceAmount(changedAmount);    
        }

        public void OnResourceDataLoaded(string resourceId, int amount)
        {
            if (_resourceComporator.ResourceId != resourceId)
                return;

            ChangeResourceAmount(amount);
        }
    }
}