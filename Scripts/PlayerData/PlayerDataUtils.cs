using UnityEngine;
using System.IO;
using TEDCore.Cipher;

namespace TEDCore.PlayerData
{
    public static class PlayerDataUtils
    {
        public static void Save<T>(T data) where T : PlayerData
        {
            string filePath = GetFilePath(CipherManager.Instance.Encrypt(data.FileName));

            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, CipherManager.Instance.Encrypt(json));
        }


        public static T Load<T>() where T : PlayerData, new()
        {
            T savableData = new T();
            string filePath = GetFilePath(CipherManager.Instance.Encrypt(savableData.FileName));

            if(!File.Exists(filePath))
            {
                TEDDebug.LogError("[PlayerDataUtils] - Finding the file failed, save the default data for player.");
                Save(savableData);
            }

            string json = File.ReadAllText(filePath);
            T playerData = JsonUtility.FromJson<T>(CipherManager.Instance.Decrypt(json));
            return playerData;
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