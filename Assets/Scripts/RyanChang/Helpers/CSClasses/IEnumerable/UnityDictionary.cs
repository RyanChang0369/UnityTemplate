using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Used to display a dictionary in the unity inspector. Use if you want to
/// allow the user to modify a dictionary that won't be changed anywhere else.
/// </summary>
/// <typeparam name="TKey">A unity-serializable value.</typeparam>
/// <typeparam name="TValue">A unity-serializable value.</typeparam>
[Serializable]
public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
    IUnityDictionary, ISerializationCallbackReceiver
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
    /// The list of <see cref="InspectorKeyValuePair"/> used by the unity editor
    /// inspector.
    /// </summary>
    [SerializeField]
    [OnValueChanged(nameof(ResetInternalDict))]
    private List<InspectorKeyValuePair> keyValuePairs;

    /// <summary>
    /// The "real" dictionary. If a change is made to the internal dictionary,
    /// it should be reflected with the keyValuePairs.
    /// </summary>
    private Dictionary<TKey, TValue> internalDict;
    #endregion

    #region Input Checking
    /// <summary>
    /// Checks if the <see cref="keyValuePairs"/> are valid and reports an error
    /// code if they are not.
    /// </summary>
    public UnityDictionaryErrorCode CalculateErrorCode()
    {
        UnityDictionaryErrorCode code = UnityDictionaryErrorCode.None;

        if (keyValuePairs == null)
        {
            return code;
        }

        if (keyValuePairs.Count != internalDict.Count)
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

    #region ISerializationCallbackReceiver Implementation
    public void OnBeforeSerialize()
    {
        // Need to save the KVPs.
        if (keyValuePairs.NotEmpty() && internalDict.NotEmpty())
        {
            // Need to keep duplicate keys as duplicate keys are automatically
            // added when keyValuePairs is added to in the inspector. ToList is
            // required to avoid changing keyValuePairs whilst the foreach loop
            // runs in SelectKeepingDuplicates().
            keyValuePairs = new(SelectKeepingDuplicates().ToList());
        }
    }

    private IEnumerable<InspectorKeyValuePair> SelectKeepingDuplicates()
    {
        // In relation to the internalDict, a key (in the KVPs) can be:
        // 1. Unique in the KVPs and unchanged.
        //    - Report it.
        // 2. Unique in the KVPs but different.
        //    - Report the one from the dictionary.
        // 3. Absent from the KVPs.
        //    - Report from the dictionary.
        // 4. Duplicated in the KVPs, with all copies matching the one in
        //    internalDict.
        //    - Report all copies as-is, but display error (handled in drawer).
        // 5. Duplicated in the KVPs, but not all copies match the one in
        //    internalDict.
        //    - Report all copies as-is, but display error (handled in drawer).

        var notInKVPs = internalDict.Keys.
            Except(
                keyValuePairs.
                    Select(kvp => kvp.Key)
            ).
            ToList();

        foreach (var kvp in keyValuePairs)
        {
            yield return kvp;
        }

        foreach (var key in notInKVPs)
        {
            yield return new (key, internalDict[key]);
        }
    }

    public void OnAfterDeserialize()
    {
        // Set the dictionary based on the KVPs.
        ResetInternalDict();
    }
    #endregion

    #region Helper Functions
    /// <summary>
    /// Resets the internal dictionary to match the <see cref="keyValuePairs"/>
    /// displayed in the editor.
    /// </summary>
    public void ResetInternalDict()
    {
        internalDict.Clear();

        foreach (var kvp in keyValuePairs)
        {
            internalDict[kvp.Key] = kvp.Value;
        }
    }
    #endregion

    #region IDictionary Implementation
    public ICollection<TKey> Keys => internalDict.Keys;

    public ICollection<TValue> Values => internalDict.Values;

    public int Count => internalDict.Count;

    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
        get
        {
            return internalDict[key];
        }
        set
        {
            internalDict[key] = value;

// #if UNITY_EDITOR
//             // This code only runs in the editor as that is the only time when
//             // keyValuePairs is modifiable.
//             int i = keyValuePairs.FindIndex(kvp => kvp.Key.Equals(key));

//             if (i >= 0)
//                 keyValuePairs[i] = new(keyValuePairs[i].Key, value);
//             else
//                 keyValuePairs.Add(new(key, value));
// #endif
        }
    }

    public void Add(TKey key, TValue value)
    {
        internalDict.Add(key, value);
        // keyValuePairs.Add(new(key, value));
    }

    public bool ContainsKey(TKey key)
    {
        return internalDict.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        bool removed = internalDict.Remove(key);

        // if (removed)
        //     keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(key));

        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return internalDict.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        internalDict.Add(item.Key, item.Value);
        // keyValuePairs.Add(new(item));
    }

    public void Clear()
    {
        internalDict.Clear();
        // keyValuePairs.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return internalDict.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)internalDict).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        bool removed = internalDict.Remove(item.Key);

        // if (removed)
        //     keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(item.Key));

        return removed;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return internalDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return internalDict.GetEnumerator();
    }
    #endregion

    public override string ToString()
    {
        var kvpStr = this.Select(d => $"({d.Key}, {d.Value})");
        return $"<{string.Join(", ", kvpStr)}>";
    }

    public void Testing()
    {
        Assert.AreEqual(
            internalDict.Count, keyValuePairs.Count,
            $"InDict ({internalDict.Count}) and " +
            $"KVPs ({keyValuePairs.Count}) different lengths!"
        );

        foreach (var kvp in keyValuePairs)
        {
            Assert.IsTrue(
                internalDict.ContainsKey(kvp.Key),
                $"{kvp.Key} not in InDict"
            );
            Assert.AreEqual(
                internalDict[kvp.Key], kvp.Value,
                "InDict/KVP values mismatch " +
                $"({internalDict[kvp.Key]} vs {kvp.Value})"
            );
        }

        Debug.Log("Tests passed");
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
    public UnityDictionaryErrorCode CalculateErrorCode();

    public void ResetInternalDict();
}
#endregion
