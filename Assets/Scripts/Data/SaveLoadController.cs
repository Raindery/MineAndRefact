using MineAndRefact.Core.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineAndRefact.Core
{
    public class SaveLoadController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private string _saveFileName = "save.data";
        [SerializeField] private List<MonoBehaviour> _saveLoadData; 
        [SerializeField] private float _saveDuration;
        [Space]
        [Header("Debug")]
        [SerializeField] private bool _debug;

        private IData _playerData;
        private IDataService _dataService;
        private readonly List<ISaveLoadHandler> _saveLoadHandlers = new List<ISaveLoadHandler>();


        private void Awake()
        {   
            _playerData = new PlayerData();
            _dataService = new DataService(Application.persistentDataPath, _saveFileName);

            if(_saveLoadData != null && _saveLoadData.Count > 0)
            {
                foreach (MonoBehaviour monoBehaviour in _saveLoadData)
                {
                    if (monoBehaviour == null)
                    {
                        Debug.LogWarning("Save load data is null!");
                        continue;
                    }

                    ISaveLoadHandler[] allHandlersOnObject = monoBehaviour.GetComponents<ISaveLoadHandler>();

                    if (allHandlersOnObject.Length > 0)
                        _saveLoadHandlers.AddRange(allHandlersOnObject);
                }
            }
        }

        private void OnEnable()
        {
            StartCoroutine(SaveWithDuration());
        }

        private void Start()
        {
            if (_dataService.DataExsist)
                Load();
        }

        
        private IEnumerator SaveWithDuration()
        {
            while (true)
            {
                yield return new WaitForSeconds(_saveDuration);
                Save();
                yield return null;
            }
        }

        public void Save()
        {
            foreach(ISaveLoadHandler handler in _saveLoadHandlers)
            {
                var collectedData = handler.CollectData();

                if (_debug)
                {
                    foreach (var d in collectedData.Data)
                    {
                        Debug.Log($"{d.Key} => {d.Value} ");
                    }
                }

                _playerData.SetData(handler.Id, collectedData);
            }



            bool saveSuccessful = _dataService.SaveData(_playerData);

            if (_debug)
            {
                if (saveSuccessful)
                    Debug.Log("Save data success!");
                else
                    Debug.LogWarning("Unable to save data!");
            }
        }

        public void Load()
        {
            IData loadedData = _dataService.LoadData<PlayerData>();

            if (loadedData == null)
                return;

            foreach(ISaveLoadHandler handler in _saveLoadHandlers)
            {
                if (loadedData.TryGetData(handler.Id, out SaveLoadData handlerData))
                    handler.SetData(handlerData);
            }
        }
    }
}