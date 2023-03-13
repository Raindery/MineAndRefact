using MineAndRefact.Core.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MineAndRefact.Core
{
    public class ResourceHolder : MonoBehaviour, ISaveLoadHandler
    {
        private const string DATA_ID = "Resources";

        [Header("Resources")]
        [SerializeField] private List<ResourceHolderData> _availableResources = new List<ResourceHolderData>();
        [Header("Gameplay Event Listener")]
        [SerializeField] private GameplayEventListener _gameplayEventListener;

        private Dictionary<string, object> _saveLoadData = new Dictionary<string, object>();

        public string Id => DATA_ID;


        private bool TryGetResourceDataById(string resourceId, out ResourceHolderData resourceHolderData)
        {
            resourceHolderData = _availableResources.FirstOrDefault((rd) => rd.Id == resourceId);

            if (resourceHolderData == null)
                return false;

            return true;

        }

        public int GetResourceAmount(string resourceId)
        {
            if (TryGetResourceDataById(resourceId, out ResourceHolderData resourceHolderData))
            {
                return resourceHolderData.Amount;
            }
            else
            {
                Debug.LogWarning($"Resource amount is not found because resource data with id - {resourceId} is not found");
                return 0;
            }
        }

        public void IncreaseResourceAmount(string resourceId, int increaseValue)
        {
            if (increaseValue < 0)
                throw new System.ArgumentException("Must have greater then 0", nameof(increaseValue));

            if(TryGetResourceDataById(resourceId, out ResourceHolderData resourceHolderData))
            {
                resourceHolderData.Amount += increaseValue;

                if (_gameplayEventListener != null)
                    _gameplayEventListener.ResourceAmountChanged.Invoke(resourceId, resourceHolderData.Amount);
            }
            else
            {
                Debug.LogWarning($"Unable increase resource amount because resource data with id - {resourceId} is not found");
            }
                
        }

        public void DecreaseResourceAmount(string resourceId, int decreaseValue)
        {
            if(decreaseValue < 0)
                throw new System.ArgumentException("Must have greater then 0", nameof(decreaseValue));

            if (TryGetResourceDataById(resourceId, out ResourceHolderData resourceHolderData))
            {
                resourceHolderData.Amount -= decreaseValue;

                if (_gameplayEventListener != null)
                    _gameplayEventListener.ResourceAmountChanged.Invoke(resourceId, resourceHolderData.Amount);
            }
            else
            {
                Debug.LogWarning($"Unable decrease resource amount because resource data with id - {resourceId} is not found");
            }
                
        }

        public SaveLoadData CollectData()
        {
            foreach(ResourceHolderData resourceHolderData in _availableResources)
            {
                _saveLoadData[resourceHolderData.Id] = resourceHolderData.Amount;
            }

            return new SaveLoadData(_saveLoadData);
        }

        public void SetData(SaveLoadData data)
        {
            int convertedAmount;

            foreach (ResourceHolderData resourceHolderData in _availableResources)
            {
                if(data.Data.TryGetValue(resourceHolderData.Id, out object amount))
                {
                    convertedAmount = System.Convert.ToInt32(amount);
                    if (convertedAmount <= 0)
                        continue;

                    resourceHolderData.Amount = convertedAmount;

                    if (_gameplayEventListener != null)
                        _gameplayEventListener.ResourceDataLoaded.Invoke(resourceHolderData.Id, convertedAmount);
                }
            }
        }


        [System.Serializable]
        private class ResourceHolderData
        {
            [SerializeField] private ResourceData _resourceComparator;
            private int _amount;

            public string Id
            {
                get
                {
                    if (_resourceComparator == null)
                        throw new System.ArgumentNullException(nameof(_resourceComparator));

                    return _resourceComparator.ResourceId;
                }
            }
            public int Amount
            {
                get
                {
                    if (_amount < 0)
                        _amount = 0;

                    return _amount;
                }

                set
                {
                    _amount = value;
                    if (_amount < 0)
                        _amount = 0;
                }
            }
        }
    }
}