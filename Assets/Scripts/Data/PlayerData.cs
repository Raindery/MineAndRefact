using System.Collections.Generic;

namespace MineAndRefact.Core.Data
{
    public class PlayerData : IData
    {
        private readonly Dictionary<string, SaveLoadData> _data;
        
        public Dictionary<string, SaveLoadData> Data => _data;


        public PlayerData()
        {
            _data = new Dictionary<string, SaveLoadData>();
        }


        public SaveLoadData GetData(string key)
        {
            return _data[key];
        }

        public void SetData(string key, SaveLoadData data)
        {
            _data[key] = data;
        }

        public bool TryGetData(string key, out SaveLoadData data)
        {
            data = new SaveLoadData();
            bool hasValue = _data.TryGetValue(key, out SaveLoadData value);

            if (!hasValue)
                return false;

            data = value;
            return true;
        }
    }
}