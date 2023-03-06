using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MineAndRefact.Core.Data;

namespace MineAndRefact.Core.UI
{
    public class UIResource : MonoBehaviour, ISaveLoadHandler
    {
        [Header("General")]
        [SerializeField] private string _id;
        [SerializeField] private ResourceType _type;
        [SerializeField] private Image _image;
        [SerializeField] private Text _amountText;
        [SerializeField] private GameplayEventListener _gameplayEventListener;

        private int _resourceAmount = 0;

        public string Id => _id;

        private void OnEnable()
        {
            if (_gameplayEventListener != null)
                _gameplayEventListener.PickUpResource.AddListener(OnPickUpResource);
        }

        private void Awake()
        {
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
            if (resource.Type == _type)
            {
                _resourceAmount++;
                _amountText.text = _resourceAmount.ToString();
            }
        }

        public SaveLoadData CollectData()
        {
            var collectedData = new Dictionary<string, object>()
            {
                {nameof(_resourceAmount),  _resourceAmount},
            };

            return new SaveLoadData(collectedData);
        }

        public void SetData(SaveLoadData data)
        {
            if (data.Data.TryGetValue(nameof(_resourceAmount), out object value))
            {
                _resourceAmount = int.Parse(value.ToString());
                _amountText.text = _resourceAmount.ToString();
            }
            else
            {
                Debug.LogWarning($"{nameof(_resourceAmount)} data is not found!");
            }
        }
    }
}