using MineAndRefact.Core.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineAndRefact.Core
{
    public class SaveLoadController : MonoBehaviour
    {
        [SerializeField] private string _pathToDataFolder;
        [SerializeField] private string _saveFileName = "save.data";
        [SerializeField] private List<MonoBehaviour> _saveLoadData; 
        [SerializeField] private GameplayEventListener _gameplayEventListener;
        [SerializeField] private float _saveDuration;
        [Space]
        [SerializeField] private bool _debug;

        private IData _playerData;
        private IDataService _dataService;
        private List<ISaveLoadHandler> _saveLoadHandlers;



        private void OnValidate()
        {
            _pathToDataFolder = Application.persistentDataPath;
        }

        private void Awake()
        {   
            _playerData = new PlayerData();
            _dataService = new DataService(_pathToDataFolder, _saveFileName);

            if(_saveLoadData != null && _saveLoadData.Count > 0)
            {
                _saveLoadHandlers = new List<ISaveLoadHandler>();

                foreach (MonoBehaviour monoBehaviour in _saveLoadData)
                {
                    ISaveLoadHandler[] allHandlersOnObject = monoBehaviour.GetComponents<ISaveLoadHandler>();

                    if (allHandlersOnObject.Length > 0)
                        _saveLoadHandlers.AddRange(allHandlersOnObject);
                }
            }

            if (_dataService.DataExsist)
                Load();
        }

        private void OnEnable()
        {
            StartCoroutine(SaveWithDuration());
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

            if (_dataService.SaveData(_playerData))
                Debug.Log("Save data success!");
            else
                Debug.LogWarning("Unable to save data!");
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


