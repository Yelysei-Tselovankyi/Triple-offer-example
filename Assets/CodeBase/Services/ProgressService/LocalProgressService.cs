using System.IO;
using CodeBase.Core.Static;
using CodeBase.Services.ProgressService.SerializableTypes;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services.ProgressService
{
    public class LocalProgressService : IProgressService
    {
        public bool IsInitialized { get; private set; }
        public PlayerProgress PlayerProgress { get; private set; }

        private readonly string _filePath = Path.Combine(Application.persistentDataPath, "playerProgress.json");

        public void Initialize()
        {  
           LoadProgress(); 
        }

        private void LoadProgress()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                PlayerProgress = JsonConvert.DeserializeObject<PlayerProgress>(json, JsonSettings.Default);
                Debug.Log("[ProgressService] Loaded progress successfully."); 
            }
            else
            {
                PlayerProgress = new PlayerProgress();
                Debug.Log("[ProgressService] Created new progress."); 
                SaveProgress();
            }
            IsInitialized = true;
        }

        public UniTask SaveProgress()
        {
            string json = JsonConvert.SerializeObject(PlayerProgress, Formatting.Indented, JsonSettings.Default);
            File.WriteAllText(_filePath, json);
            Debug.Log("[ProgressService] Progress saved.");
            return default;
        }
    }
}