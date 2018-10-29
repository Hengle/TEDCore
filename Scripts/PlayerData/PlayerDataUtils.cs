using UnityEngine;
using System.IO;

namespace TEDCore.PlayerData
{
    public static class PlayerDataUtils
    {
        public static void Save<T>(T data) where T : PlayerData
        {
            string filePath = GetFilePath(data.FileName);

            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }


        public static T Load<T>() where T : PlayerData, new()
        {
            T savableData = new T();
            string filePath = GetFilePath(savableData.FileName);

            if(!File.Exists(filePath))
            {
                Save(savableData);
            }

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }


        private static string GetFilePath(string fileName)
        {
            return GetPlayerDataPath() + fileName;
        }


        private static string GetPlayerDataPath()
        {
            return GetPersistentDataPath() + "/PlayerData/";
        }


        public static string GetPersistentDataPath()
        {
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets", "");
#else
            return Application.persistentDataPath;
#endif
        }
    }

}