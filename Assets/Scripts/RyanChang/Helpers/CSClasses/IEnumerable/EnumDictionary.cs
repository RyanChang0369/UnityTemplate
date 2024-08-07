using System;

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
    public override bool AssignEditorDictionary(UnityEngine.Object targetObject)
    {
        bool noChanges = true;

        foreach (TEnum enumIndex in EnumExt.GetValues<TEnum>())
        {
            if (!editorDict.ContainsKey(enumIndex))
            {
                // No key. Fix it.
                editorDict[enumIndex] = default;
                noChanges = false;
            }
        }

        return noChanges;
    }

    public override string LabelFromKey(TEnum key)
    {
        return Enum.GetName(typeof(TEnum), key);
    }
    #endregion

    #region Public Methods
    /// <inheritdoc cref="UnityDictionary{TKey, TValue}.ResetInternalDict"/>
    public void ResetInternalDict() => editorDict.ResetInternalDict();
    #endregion
}
