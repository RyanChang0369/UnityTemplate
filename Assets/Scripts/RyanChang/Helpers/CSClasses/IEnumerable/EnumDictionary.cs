using System;
using System.Collections.Generic;

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
    public EnumDictionary()
    {
        foreach (TEnum enumIndex in EnumExt.GetValues<TEnum>())
        {
            editorDict[enumIndex] = default;
        }
    }

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
