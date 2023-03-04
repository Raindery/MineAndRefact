using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MineAndRefact.Core.UI
{
    public class UIResource : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private ResourceType _type;
        [SerializeField] private Image _image;
        [SerializeField] private Text _amountText;
        [SerializeField] private GameplayEventListener _gameplayEventListener;


        private int _resourceAmount;


        private void OnEnable()
        {
            if (_gameplayEventListener != null)
                _gameplayEventListener.PickUpResource.AddListener(OnPickUpResource);
        }

        private void Awake()
        {
            _resourceAmount = 0;

            if (_amountText != null)
                _amountText.text = _resourceAmount.ToString();
        }

        private void OnDisable()
        {
            if (_gameplayEventListener != null)
                _gameplayEventListener.PickUpResource.RemoveListener(OnPickUpResource);
        }

        private void OnPickUpResource(IResource resource)
        {

            if(resource.Type == _type)
            {
                _resourceAmount++;
                _amountText.text = _resourceAmount.ToString();
            }
                
        }
    }
}

