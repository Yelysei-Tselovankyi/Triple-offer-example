using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.ResourcesProvider
{
    public interface IAddressableAssetProvider
    {
        UniTask<T> LoadAsync<T>(string key = null) where T : Object;
        void Release<T>(T asset) where T : Object;
        void ReleaseInstance(GameObject asset);

        UniTask<GameObject> InstantiateAsync(
            object key,
            Transform parent = null,
            bool instantiateInWorldSpace = false,
            bool trackHandle = true);

        UniTask<T> LoadAsync<T>(AssetReference key) where T : Object;
    }
}