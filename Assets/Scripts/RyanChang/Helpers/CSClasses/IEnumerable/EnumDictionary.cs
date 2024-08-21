using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a dictionary of enums, where each enum is mapped to a value.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[Serializable]
public class EnumDictionary<TEnum, TValue> :
    StaticKeyedDictionary<TEnum, TValue>
    where TEnum : struct, Enum
{
    #region Constructors
    private static IDictionary<TEnum, TValue> DefaultEnumDict =>
        EnumExt.GetValues<TEnum>().
        ToDictionary<TEnum, TEnum, TValue>(d => d, d => default);

    public EnumDictionary() : base(DefaultEnumDict)
    { }

    protected EnumDictionary(IDictionary<TEnum, TValue> dict) : base(dict)
    { }

    #endregion

    #region StackedKeyDictionary Implementation
    public override void GenerateStaticKeys(UnityEngine.Object targetObject)
    {
        foreach (TEnum enumIndex in EnumExt.GetValues<TEnum>())
        {
            // If key missing, then adds it with a default value.
            editorDict.TryAdd(enumIndex, default);
        }
    }

    public override string LabelFromKey(TEnum key)
    {
        return Enum.GetName(typeof(TEnum), key);
    }
    #endregion
}
