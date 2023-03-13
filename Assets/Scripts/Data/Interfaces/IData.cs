using System.Collections.Generic;

namespace MineAndRefact.Core.Data
{
    public interface IData
    {
        Dictionary<string, SaveLoadData> Data { get; }

        bool TryGetData(string key, out SaveLoadData data);
        SaveLoadData GetData(string key);
        void SetData(string key, SaveLoadData data);
    }
}