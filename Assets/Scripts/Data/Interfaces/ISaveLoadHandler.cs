using System.Collections.Generic;

namespace MineAndRefact.Core.Data
{
    public interface ISaveLoadHandler
    {
        string Id { get; }

        SaveLoadData CollectData();
        void SetData(SaveLoadData data);
    }

    [System.Serializable]
    public struct SaveLoadData
    {
        private Dictionary<string, object> _data;

        public Dictionary<string, object> Data => _data;


        public SaveLoadData(Dictionary<string, object> data)
        {
            _data = data;
        }
    }
}