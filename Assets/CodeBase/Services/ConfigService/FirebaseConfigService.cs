using System;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace CodeBase.Services.ConfigService
{
    public class FirebaseConfigService : IConfigService
    {
        private AsyncLazy _configsLoadingTask;
        private FirebaseRemoteConfig _remoteConfig;

        public async UniTask<T> LoadAsync<T>(string id = null) where T : IConfig
        {
            id ??= typeof(T).Name;

            _configsLoadingTask ??= new AsyncLazy(LoadRemoteConfigs);
            
            await _configsLoadingTask;
            
            string json = _remoteConfig.GetValue(id).StringValue;
            T config = JsonUtility.FromJson<T>(json);
            return config;
        }

        private async UniTask LoadRemoteConfigs()
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

            _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            ConfigInfo info = _remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"[FirebaseConfigService] Fetch unsuccessful: {info.LastFetchStatus}");
                return;
            }

            await _remoteConfig.ActivateAsync();
            Debug.Log($"[FirebaseConfigService] Remote data loaded and ready. Last fetch: {info.FetchTime}");
        }
    }
}