using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine.Assertions;

/// <summary>
/// <see cref="JsonConverter"/> for <see
/// cref="StaticKeyedDictionary{TKey, TValue}"/>.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public class StaticKeyedDictionaryConverter : UnityDictionaryConverter
{
    protected override Type DictionaryType => typeof(StaticKeyedDictionary<,>);
    
    public override object ReadJson(JsonReader reader, Type objectType,
        object existingValue, JsonSerializer serializer)
    {
        Type[] generics = GetBaseGenericTypeArguments(objectType);

        Type jsonPreloadType = typeof(IDictionary<,>).
            MakeGenericType(typeof(string), generics[1]);
        Type dictType = typeof(IDictionary);
        Type unityDictType = DictionaryType.
            MakeGenericType(generics);

        // object jsonValues = serializer.Deserialize(reader, jsonPreloadType);
        object test = serializer.Deserialize(reader, dictType);
        var constructorInfo = objectType.GetConstructor(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            Type.DefaultBinder,
            new Type[] { },
            new ParameterModifier[] { }
        );

        Assert.IsNotNull(
            test,
            "JSON deserialization failure:"
        );

        Assert.IsNotNull(
            constructorInfo,
            "Default constructor not found for " + objectType.Name
        );

        object value = constructorInfo.Invoke(
            new object[] { }
        );

        return value;
    }
}
