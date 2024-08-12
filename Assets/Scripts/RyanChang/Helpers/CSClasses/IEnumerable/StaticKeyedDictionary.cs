
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides a base class to allow for easy creation of new dictionaries (with
/// program-defined keys) that are visible to the unity editor. Utilizes the
/// <see cref="UnityDictionary{TKey, TValue}"/>
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
public abstract class StaticKeyedDictionary<TKey, TValue> :
    IDictionary<TKey, TValue>, IStaticKeyedDictionary
{
    #region Variables
    /// <summary>
    /// The dictionary explicitly used by the editor.
    /// </summary>
    [SerializeField]
    protected UnityDictionary<TKey, TValue> editorDict = new();
    #endregion

    #region IStaticKeyedDictionary Implementation
    public abstract void GenerateStaticKeys(UnityEngine.Object targetObject);

    /// <inheritdoc cref="LabelFromKey(object)"/>
    public abstract string LabelFromKey(TKey key);

    public string LabelFromKey(object key)
    {
        if (key is TKey genericKey)
        {
            return LabelFromKey(genericKey);
        }

        throw new ArgumentException($"{key} not of correct type ({typeof(TKey)}).");
    }

    public UnityDictionaryErrorCode CalculateErrorCode() =>
        editorDict.CalculateErrorCode();

    public void ResetInternalDict() => editorDict.ResetInternalDict();

    public void ResetInspectorKVPs() => editorDict.ResetInspectorKVPs();
    #endregion

    #region IDictionary Implementation
    public TValue this[TKey key]
    {
        get => editorDict[key];
        set => editorDict[key] = value;
    }

    public ICollection<TKey> Keys => editorDict.Keys;

    public ICollection<TValue> Values => editorDict.Values;

    public int Count => editorDict.Count;

    public bool IsReadOnly => editorDict.IsReadOnly;

    public void Add(TKey key, TValue value) => editorDict.Add(key, value);

    public void Add(KeyValuePair<TKey, TValue> item) => editorDict.Add(item);

    public void Clear() => editorDict.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) =>
        editorDict.Contains(item);

    public bool ContainsKey(TKey key) => editorDict.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
        editorDict.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        editorDict.GetEnumerator();

    public bool Remove(TKey key) => editorDict.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item) =>
        editorDict.Remove(item);

    public bool TryGetValue(TKey key, out TValue value) =>
        editorDict.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => editorDict.GetEnumerator();
    #endregion

    public override string ToString() => editorDict.ToString();
}
