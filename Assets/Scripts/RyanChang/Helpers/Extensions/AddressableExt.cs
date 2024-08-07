#if AddressableAssets
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    
    
    /// <summary>
    /// Extensions for <see cref="Addressables"/>
    /// </summary>
    /// 
    /// <remarks>
    /// Authors: Ryan Chang (2024)
    /// </remarks>
    public static class AddressableExt
    {
        /// <summary>
        /// Loads the addressable <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to load. If loading a prefab, this is
        /// most likely <see cref="GameObject"/>.</typeparam>
        /// <param name="key">Name/label/path of the addressable to load.</param>
        /// <param name="onSuccess">Action performed when/if loading
        /// succeeds.</param>
        /// <param name="onFail">Action performed when/if loading fails.</param>
        public static void LoadAddressable<T>(string key,
            Action<string, T> onSuccess, Action<string> onFail)
        {
            if (string.IsNullOrEmpty(key))
            {
                onFail?.Invoke(key);
            }
            else
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
    
                handle.Completed += (h) =>
                {
                    switch (h.Status)
                    {
                        case AsyncOperationStatus.Succeeded:
                            onSuccess(key, handle.Result);
                            break;
                        default:
                            onFail?.Invoke(key);
                            break;
                    }
    
                    Addressables.Release(h);
                };
            }
        }
    
        /// <inheritdoc cref="LoadAddressable{T}(string, Action{string, T},
        /// Action{string})"/>
        public static void LoadAddressable<T>(string name,
            Action<string, T> onSuccess) =>
            LoadAddressable(name, onSuccess, null);
    }
    
#endif