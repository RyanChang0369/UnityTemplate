using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Used to display a dictionary in the unity inspector. Use if you want to
/// allow the user to modify a dictionary that won't be changed anywhere else.
/// </summary>
/// <typeparam name="TKey">A unity-serializable value.</typeparam>
/// <typeparam name="TValue">A unity-serializable value.</typeparam>
[Serializable]
public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
    IUnityDictionary
{
    #region Structs
    [Serializable]
    public struct InspectorKeyValuePair
    {
        #region Variables
        /// <summary>
        /// The key associated with this <see cref="InspectorKeyValuePair"/>.
        /// </summary>
        [SerializeField]
        private TKey key;

        /// <summary>
        /// The value associated with this <see cref="InspectorKeyValuePair"/>.
        /// </summary>
        [SerializeField]
        private TValue value;
        #endregion

        #region Properties
        /// <inheritdoc cref="key"/>
        public readonly TKey Key => key;

        /// <inheritdoc cref="value"/>
        public readonly TValue Value => value;
        #endregion

        public InspectorKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        /// <summary>
        /// Creates a new <see cref="InspectorKeyValuePair"/> from <paramref
        /// name="keyValuePair"/>.
        /// </summary>
        /// <param name="keyValuePair">The <see cref="KeyValuePair{TKey,
        /// TValue}"/> to use.</param>
        public InspectorKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
        {
            this.key = keyValuePair.Key;
            this.value = keyValuePair.Value;
        }
    }
    #endregion

    #region Variables
    /// <summary>
    /// The list of <see cref="InspectorKeyValuePair"/> used by the inspector.
    /// </summary>
    [SerializeField]
    [OnValueChanged(nameof(ResetInternalDict))]
    private List<InspectorKeyValuePair> keyValuePairs;
    private int prevKVPsHash;

    /// <summary>
    /// The "real" dictionary. If a change is made to the internal dictionary,
    /// it should be reflected with the keyValuePairs.
    /// </summary>
    private Dictionary<TKey, TValue> internalDict;

    private Dictionary<TKey, TValue> BufferedDict
    {
        get
        {
            // The function only regenerates the internal dictionary if a
            // difference has been found.
            ValidateInternalDict();
            return internalDict;
        }
    }
    #endregion

    #region Input Checking
    /// <summary>
    /// Checks if the <see cref="keyValuePairs"/> are valid.
    /// </summary>
    public UnityDictionaryErrorCode ValidateKVPs()
    {
        UnityDictionaryErrorCode code = UnityDictionaryErrorCode.None;

        if (keyValuePairs == null)
        {
            return code;
        }

        if (keyValuePairs.Count != keyValuePairs.Distinct(inspectorKVPKeyComparer).Count())
        {
            code |= UnityDictionaryErrorCode.DuplicateKeys;
        }

        if (typeof(TKey).IsClass)
        {
            if (keyValuePairs.Any(kvp => kvp.Key == null))
            {
                code |= UnityDictionaryErrorCode.NullKeys;
            }
        }

        return code;
    }

    private static readonly InspectorKVPKeyComparer inspectorKVPKeyComparer = new();

    private class InspectorKVPKeyComparer : IEqualityComparer<InspectorKeyValuePair>
    {
        public bool Equals(InspectorKeyValuePair x, InspectorKeyValuePair y)
        {
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(InspectorKeyValuePair obj)
        {
            return obj.GetHashCode();
        }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new empty UnityDictionary.
    /// </summary>
    public UnityDictionary()
    {
        keyValuePairs = new();
        internalDict = new();
    }

    /// <summary>
    /// Creates a new UnityDictionary from the collection of key value pairs.
    /// </summary>
    /// <param name="collection">The aforementioned collection.</param>
    public UnityDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection.IsNullOrEmpty())
        {
            // No items in collection
            keyValuePairs = new();
            internalDict = new();
        }
        else
        {
            // There are items in the collection.
            keyValuePairs = new(collection.Select(
                c => new InspectorKeyValuePair(c)
            ));
            internalDict = new(collection);
        }
    }
    #endregion

    #region Helper Functions
    /// <summary>
    /// Determines if the internal dictionary matches <see
    /// cref="keyValuePairs"/>, and regenerates it if not.
    /// </summary>
    /// <returns>True if update was performed.</returns>
    private bool ValidateInternalDict()
    {
        // if (keyValuePairs.NotEmpty())
        // {
        //     // Check the hash to avoid unneeded updates. 
        //     int KVPsHash = keyValuePairs.GetHashCode();
        //     if (KVPsHash != prevKVPsHash)
        //     {
        //         ResetInternalDict(KVPsHash);
        //         Debug.LogWarning(
        //             "Difference in InternalDict found " +
        //             $"[{keyValuePairs.Count}, {internalDict.Count}]"
        //         );
        //         return true;
        //     }
        // }

        return false;
    }

    /// <summary>
    /// Resets the internal dictionary to match the <see cref="keyValuePairs"/>
    /// displayed in the editor.
    /// </summary>
    /// <param name="hash">If non-zero, use this as a hash for the <see
    /// cref="prevKVPsHash"/>. Otherwise, generate the hash from <see
    /// cref="keyValuePairs"/>.</param>
    public void ResetInternalDict(int hash = 0)
    {
        // Clear();

        // foreach (var keyValue in keyValuePairs)
        // {
        //     internalDict[keyValue.Key] = keyValue.Value;
        // }

        // if (hash == 0)
        //     prevKVPsHash = keyValuePairs.GetHashCode();
        // else
        //     prevKVPsHash = hash;

        internalDict = keyValuePairs.ToDictionary(
            (kvp) => kvp.Key,
            (kvp) => kvp.Value
        );
    }
    #endregion

    #region IDictionary Implementation
    public ICollection<TKey> Keys => BufferedDict.Keys;

    public ICollection<TValue> Values => BufferedDict.Values;

    public int Count => BufferedDict.Count;

    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
        get
        {
            return BufferedDict[key];
        }
        set
        {
            BufferedDict[key] = value;

#if UNITY_EDITOR
            // This code only runs in the editor as that is the only time when
            // keyValuePairs is modifiable.
            int i = keyValuePairs.FindIndex(kvp => kvp.Key.Equals(key));

            if (i >= 0)
                keyValuePairs[i] = new(keyValuePairs[i].Key, value);
            else
                keyValuePairs.Add(new(key, value));
#endif
        }
    }

    public void Add(TKey key, TValue value)
    {
        BufferedDict.Add(key, value);
        keyValuePairs.Add(new(key, value));
    }

    public bool ContainsKey(TKey key)
    {
        return BufferedDict.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        bool removed = BufferedDict.Remove(key);

        if (removed)
            keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(key));

        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return BufferedDict.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        BufferedDict.Add(item.Key, item.Value);
        keyValuePairs.Add(new(item));
    }

    public void Clear()
    {
        BufferedDict.Clear();
        keyValuePairs.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return BufferedDict.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)BufferedDict).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        bool removed = BufferedDict.Remove(item.Key);

        if (removed)
            keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(item.Key));

        return removed;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return BufferedDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return BufferedDict.GetEnumerator();
    }
    #endregion

    public override string ToString()
    {
        var kvpStr = this.Select(d => $"({d.Key}, {d.Value})");
        return $"<{string.Join(", ", kvpStr)}>";
    }
}

#region Enums
[Flags]
public enum UnityDictionaryErrorCode
{
    /// <summary>
    /// <see cref="keyValuePairs"/> has been successfully validated.
    /// </summary>
    None = 0,
    /// <summary>
    /// <see cref="keyValuePairs"/> has at least 2 duplicate keys.
    /// </summary>
    DuplicateKeys = 1,
    /// <summary>
    /// <see cref="keyValuePairs"/> has at least 1 null key.
    /// </summary>
    NullKeys = 2
}
#endregion

#region Interfaces
/// <summary>
/// Interface used solely by the <see cref="UnityDictionary<,>"/>, to allow
/// certain methods to be used without requiring the specification of type
/// params.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public interface IUnityDictionary
{
    public UnityDictionaryErrorCode ValidateKVPs();
}
#endregion
