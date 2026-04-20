using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.ConfigService
{
    public class LocalConfigService : IConfigService
    {
        private readonly string _configFilesPath = Application.streamingAssetsPath;
        private readonly Dictionary<string, IConfig> _cachedConfigs = new ();
        
        public async UniTask<T> LoadAsync<T>(string id = null) where T : IConfig
        {
            id ??= typeof(T).Name;

            if (_cachedConfigs.TryGetValue(id, out IConfig cachedConfig))
            {
                return (T)cachedConfig;    
            }
            
            string path = Path.Combine(_configFilesPath, id + ".json");

            string json = await LoadJsonAsync(path);
            T config = JsonUtility.FromJson<T>(json);
            _cachedConfigs[id] = config;
            return config;
        }

       private async UniTask<string> LoadJsonAsync(string path)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (UnityWebRequest request = UnityWebRequest.Get(path))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception($"WebRequest failed: {request.error}");
                }

                return request.downloadHandler.text;
            }
#else
            // Для других платформ: обычный File.ReadAllText, обёрнутый в UniTask
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            return await File.ReadAllTextAsync(path);
#endif
        }
    }
}