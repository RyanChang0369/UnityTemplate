using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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
[JsonObject(MemberSerialization.OptIn)]
[Serializable]
public abstract class StaticKeyedDictionary<TKey, TValue> :
    IReadOnlyDictionary<TKey, TValue>, IStaticKeyedDictionary
{
    #region Variables
    /// <summary>
    /// The dictionary explicitly used by the editor.
    /// </summary>
    [JsonProperty, SerializeField]
    protected UnityDictionary<TKey, TValue> editorDict = new();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="StaticKeyedDictionary{TKey, TValue}"/> from the
    /// provided <paramref name="dictionary"/>.
    /// </summary>
    /// <remarks>
    /// This constructor is marked protected as it should only be used by the
    /// JSON converter, and this constructor should not be used normally. See
    /// the documentation for <see cref="ISaveLoadable{T}"/> for more
    /// information on JSON.
    /// </remarks>
    /// <param name="dictionary">The provided dictionary.</param>
    [JsonConstructor]
    protected StaticKeyedDictionary(IDictionary<TKey, TValue> dictionary)
    {
        editorDict = new(dictionary);
    }
    #endregion

    #region Properties
    #region IReadOnlyDictionary Implementation
    public IEnumerable<TKey> Keys => editorDict.Keys;

    public IEnumerable<TValue> Values => editorDict.Values;

    public int Count => editorDict.Count;
    #endregion
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

    public IDictionary AsDictionary() => editorDict.AsDictionary();

    public UnityDictionaryErrorCode CalculateErrorCode() =>
        editorDict.CalculateErrorCode();

    public void ResetInternalDict(bool force = false) =>
        editorDict.ResetInternalDict(force);

    public void ResetInspectorKVPs(bool force = false) =>
        editorDict.ResetInspectorKVPs(force);
    #endregion

    #region IReadOnlyDictionary Implementation
    public TValue this[TKey key]
    {
        get => editorDict[key];
        set => editorDict[key] = value;
    }

    public bool ContainsKey(TKey key) => editorDict.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) =>
        editorDict.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        editorDict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => editorDict.GetEnumerator();
    #endregion

    #region Other Methods
    public override string ToString() => editorDict.ToString();
    #endregion
}
