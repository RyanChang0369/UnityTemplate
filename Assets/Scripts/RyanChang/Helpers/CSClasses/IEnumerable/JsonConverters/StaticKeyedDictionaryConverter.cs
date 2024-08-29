using System;

/// <summary>
/// <see cref="Newtonsoft.Json.JsonConverter"/> for <see
/// cref="StaticKeyedDictionary{TKey, TValue}"/>.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public class StaticKeyedDictionaryConverter : UnityDictionaryConverter
{
    protected override Type DictionaryType => typeof(StaticKeyedDictionary<,>);
}
