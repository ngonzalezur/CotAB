using UnityEngine;
using System.IO;

namespace CotA.Configuration
{
    public static class FileConfigurationLoader
    {
        public const string FileName = "Balance.json";

        public static ConfigurationData TryLoadFromFile()
        {
            string filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                ConfigurationData jsonObject = JsonUtility.FromJson<ConfigurationData>(jsonText);
                return jsonObject;
            }
            else
                return null;
        }

        public static void SaveToFile(ConfigurationData data)
        {
            string filePath = GetFilePath();
            string jsonText = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonText);
        }

        private static string GetFilePath()
        {
            return string.Format("{0}/{1}", Application.dataPath, FileName);
        }
    }
}