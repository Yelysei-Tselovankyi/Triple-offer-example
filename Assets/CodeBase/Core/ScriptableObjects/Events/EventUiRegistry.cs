using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Core.ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "EventUiRegistry", menuName = "Events/Event UI Registry")]
    public class EventUiRegistry : ScriptableObject
    {
        [SerializeField] private List<EventUiEntry> Entries;
        
        private Dictionary<string, AssetReferenceGameObject> _buttonMap;
        private Dictionary<string, AssetReferenceGameObject> _windowMap;

        public AssetReferenceGameObject GetButton(string id) {
            _buttonMap ??= Entries.ToDictionary(e => e.EventId, e => e.ButtonPrefab);
            return _buttonMap[id];
        }

        public AssetReferenceGameObject GetWindow(string id)
        {
            _windowMap ??= Entries.ToDictionary(e => e.EventId, e => e.WindowPrefab);
            return _windowMap[id];
        }
    }
}