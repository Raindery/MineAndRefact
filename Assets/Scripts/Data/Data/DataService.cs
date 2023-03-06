using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace MineAndRefact.Core.Data
{
    public class DataService : IDataService
    {
        private readonly string _pathToDataFolder;
        private readonly string _fileName;
        private readonly string _fullPath;

        public string PathToDataFolder => _pathToDataFolder;
        public string FileName => _fileName;
        public string FullPath => _fullPath;
        public bool DataExsist => File.Exists(_fullPath);


        public DataService(string pathToDataFolder, string fileName)
        {
            _pathToDataFolder = pathToDataFolder;
            _fileName = fileName;

            _fullPath = Path.Combine(_pathToDataFolder, fileName);
        }


        public bool SaveData<T>(T data) where T : IData
        {
            try
            {
                using(StreamWriter streamWriter = new StreamWriter(_fullPath, false))
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(data));
                }
                return true;
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Unable to save data due to \n {e.Message} \n {e.StackTrace}");
                return false;
            }
        }

        public T LoadData<T>() where T : IData
        {
            try
            {
                using(StreamReader streamReader = new StreamReader(_fullPath))
                {
                    string dataString = streamReader.ReadLine();
                    return JsonConvert.DeserializeObject<T>(dataString);
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Unable to load data due to \n {e.Message} \n {e.StackTrace}");
                return default;
            }
        }
    }
}