namespace BiruisredEngine
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    [System.Serializable]
    internal class IDCacheData
    {
        public List<string> ids = new();
    }

    public static class IDGenerator
    {
        private static readonly string cachePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "CacheIDs.json");
        private static HashSet<string> idSet;
        private static IDCacheData cacheData;

        // Ensure cache is loaded
        private static void EnsureLoaded()
        {
            if (idSet != null) return;

            if (File.Exists(cachePath))
            {
                string json = File.ReadAllText(cachePath);
                cacheData = JsonUtility.FromJson<IDCacheData>(json);
            }
            else
            {
                cacheData = new IDCacheData();
            }

            idSet = new HashSet<string>(cacheData.ids);
        }

        // Save cache back to file
        private static void Save()
        {
            cacheData.ids = new List<string>(idSet);
            string json = JsonUtility.ToJson(cacheData, true);
            File.WriteAllText(cachePath, json);
        }

        /// <summary>
        /// Generate a unique ID with optional prefix, suffix, and custom length.
        /// Example: Generate("ITEM_", "_EU", 5) -> "ITEM_AB12X_EU"
        /// </summary>
        public static string Generate(string prefix = "", string suffix = "", int length = 6)
        {
            EnsureLoaded();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string newId;

            do
            {
                char[] buffer = new char[length];
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = chars[Random.Range(0, chars.Length)];
                }

                newId = prefix + new string(buffer) + suffix;
            }
            while (idSet.Contains(newId));

            idSet.Add(newId);
            Save();

            return newId;
        }
    }

}