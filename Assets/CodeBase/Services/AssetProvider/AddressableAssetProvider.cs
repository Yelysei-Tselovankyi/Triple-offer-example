using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CodeBase.Services.AssetProvider
{
    public class AddressableAssetProvider : IAddressableAssetProvider
    {
        public async UniTask<T> LoadAsync<T>(string key = null) where T : Object
        {
            key ??= typeof(T).Name;
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            return await LoadFromHandleAsync(handle, key);
        }

        public async UniTask<T> LoadAsync<T>(AssetReference key) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            return await LoadFromHandleAsync(handle, key);
        }

        public async UniTask<GameObject> InstantiateAsync(
            object key,
            Transform parent = null,
            bool instantiateInWorldSpace = false,
            bool trackHandle = true)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(
                key,
                parent: parent,
                instantiateInWorldSpace: instantiateInWorldSpace,
                trackHandle: trackHandle);

            await handle.ToUniTask();

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new FileNotFoundException(
                    $"[AddressableResourceProvider] Failed to instantiate addressable {key}");

            return handle.Result;
        }

        public void Release<T>(T asset) where T : Object
        {
            CheckAssetForNull(asset);
            Addressables.Release(asset);
        }

        public void ReleaseInstance(GameObject asset)
        {
            CheckAssetForNull(asset);
            Addressables.ReleaseInstance(asset);
        }

        private void CheckAssetForNull<T>(T asset) where T : Object
        {
            if (asset == null)
                throw new NullReferenceException(
                    $"[AddressableResourceProvider] Failed to release addressable {typeof(T).Name}");
        }

        private async UniTask<T> LoadFromHandleAsync<T>(AsyncOperationHandle<T> handle, object key) where T : Object
        {
            await handle.ToUniTask();

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new FileNotFoundException($"[AddressableResourceProvider] Failed to load addressable {key}");

            return handle.Result;
        }
    }
}