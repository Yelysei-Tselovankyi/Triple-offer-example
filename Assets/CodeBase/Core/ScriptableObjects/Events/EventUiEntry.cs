using System;
using UnityEngine.AddressableAssets;

namespace CodeBase.Core.ScriptableObjects.Events
{
    [Serializable]
    public class EventUiEntry
    {
        public string EventId;
        public AssetReferenceGameObject ButtonPrefab;
        public AssetReferenceGameObject WindowPrefab;
    }
}