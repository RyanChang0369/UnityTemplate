using System;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// <see cref="JsonConverter"/> for <see cref="UnityDictionary{TKey, TValue}"/>.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public class UnityDictionaryConverter : JsonConverter
{
    protected virtual Type DictionaryType => typeof(UnityDictionary<,>);

    public override bool CanConvert(Type objectType)
    {
        if (objectType.IsGenericType)
        {
            var generics = objectType.GenericTypeArguments;

            if (generics.Length == 2)
            {
                var unityDictType = DictionaryType.
                    MakeGenericType(generics);
                return objectType.Equals(unityDictType);
            }
        }

        return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType,
        object existingValue, JsonSerializer serializer)
    {
        var generics = objectType.GenericTypeArguments;
        var dictType = typeof(IDictionary<,>).
            MakeGenericType(generics);
        var unityDictType = DictionaryType.
            MakeGenericType(generics);

        object value = unityDictType.GetConstructor(
            new Type[] { dictType }
        ).Invoke(
            new object[] { serializer.Deserialize(reader, dictType) }
        );

        return value;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Type objectType = value.GetType();
        var generics = objectType.GenericTypeArguments;

        var dictType = typeof(Dictionary<,>).
            MakeGenericType(generics);
        var iDictType = typeof(IDictionary<,>).
            MakeGenericType(generics);

        object dictionary = dictType.GetConstructor(
            new Type[] { iDictType }
        ).Invoke(
            new object[] { value }
        );

        serializer.Serialize(writer, dictionary);
    }
}
