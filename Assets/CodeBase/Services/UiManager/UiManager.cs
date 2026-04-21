using System.Collections.Generic;
using CodeBase.Core.ScriptableObjects.Events;
using CodeBase.Core.UI;
using CodeBase.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Services.UiManager
{
    public class UiManager : IUiManager
    {
        private readonly Dictionary<string, BaseWindow> _openWindows = new();
        private readonly EventUiRegistry _eventUiRegistry;
        private readonly IAddressableAssetProvider _addressableAssetProvider;

        [Inject]
        public UiManager(
            IAddressableAssetProvider addressableAssetProvider,
            EventUiRegistry eventUiRegistry)
        {
            _eventUiRegistry = eventUiRegistry;
            _addressableAssetProvider = addressableAssetProvider;
        }

        public async UniTask<T> ShowWindow<T>(string id = null) where T : BaseWindow
        {
            id ??= typeof(T).Name;
            GameObject instance = await _addressableAssetProvider.InstantiateAsync(id);
            T uiWindow = instance.GetComponent<T>();
            uiWindow.PlayOpenAnimation();
            _openWindows.Add(id, uiWindow);
            return uiWindow;
        }

        public async UniTask CloseWindow<T>(string id = null) where T : BaseWindow
        {
            id ??= typeof(T).Name;
            BaseWindow window = _openWindows[id];
            _openWindows.Remove(id);
            await window.PlayCloseAnimation();
            _addressableAssetProvider.ReleaseInstance(window.gameObject);
        }

        public async UniTask<T> ShowEventWindow<T>(string eventId) where T : BaseWindow
        {
            AssetReferenceGameObject windowRef = _eventUiRegistry.GetWindow(eventId);
            GameObject instance = await _addressableAssetProvider.InstantiateAsync(windowRef);
            T uiWindow = instance.GetComponent<T>();
            uiWindow.PlayOpenAnimation();
            _openWindows.Add(eventId, uiWindow);
            return uiWindow;
        }

        public async UniTask CloseEventWindow(string eventId)
        {
            BaseWindow window = _openWindows[eventId];
            _openWindows.Remove(eventId);
            await window.PlayCloseAnimation();
            _addressableAssetProvider.ReleaseInstance(window.gameObject);
        }
    }
}