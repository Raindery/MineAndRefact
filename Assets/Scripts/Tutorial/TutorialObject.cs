using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MineAndRefact.Core
{
    public class TutorialObject : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Image _pointerImage;
        [Space]
        [Header("Animation Settings")]
        [SerializeField] private float _offsetY = 1f;
        [SerializeField] private float _duration = 0.5f;

        private Sequence _poinertAnimationSequince;


        private void Awake()
        {
            _pointerImage.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_poinertAnimationSequince.IsActive())
                _poinertAnimationSequince.Kill();
        }


        public void ShowPointer()
        {
            if(_pointerImage != null)
            {
                _pointerImage.gameObject.SetActive(true);

                _poinertAnimationSequince = DOTween.Sequence()
                    .Append(_pointerImage.transform.DOMoveY(_pointerImage.transform.position.y - _offsetY, _duration))
                    .Append(_pointerImage.transform.DOMoveY(_pointerImage.transform.position.y, _duration))
                    .AppendInterval(0.5f)
                    .SetLoops(-1);
            }
        }

        public void HidePointer()
        {
            if(_poinertAnimationSequince.IsActive())
                _poinertAnimationSequince.Kill();

            if (_pointerImage != null)
                _pointerImage.gameObject.SetActive(false);
        }
    }
}