namespace MineAndRefact.Core.Data
{
    public interface IDataService
    {
        string PathToDataFolder { get; }
        string FileName { get; }
        string FullPath { get; }
        bool DataExsist { get; }

        bool SaveData<T>(T data) where T : IData;
        T LoadData<T>() where T : IData;
    }
}